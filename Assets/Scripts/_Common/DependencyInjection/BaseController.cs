using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Zenject.Helpers
{
    public abstract class BaseController : IInitializable, ITickable, IDisposable, IActivatable
    {
        #region Lifecycle

        private bool _isInitialized;
        private bool _isEnabled;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (!_isInitialized || _isEnabled == value)
                {
                    return;
                }
                
                _isEnabled = value;

                if (value)
                {
                    OnEnabled();
                }
                else
                {
                    OnDisabled();
                }
            }
        }

        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            
            OnInitialized();
        
            foreach (var controller in _controllers)
            {
                controller.Initialize();
            }
            
            _isInitialized = true;
            IsEnabled = true;
        }

        public void Tick()
        {
            if (!IsEnabled)
            {
                return;
            }
            
            OnTick();
        
            // TODO: optimize this to avoid iterating over all controllers every tick
            foreach (var controller in _controllers)
            {
                controller.Tick();
            }
        }

        public void Dispose()
        {
            UnsubscribeAll();
            RemoveAllControllers();
        
            IsEnabled = false;
            OnDisposed();
        }
    
        protected virtual void OnInitialized()
        {
        }
        
        protected virtual void OnDisposed()
        {
        }
        
        protected virtual void OnEnabled()
        {
        }
        
        protected virtual void OnDisabled()
        {
        }
        
        protected virtual void OnTick()
        {
        }

        #endregion


        #region Signals

        [Inject] private readonly SignalBus _signalBus;
        private readonly List<(Type, object)> _subscriptions = new();
    
        protected void Subscribe<TSignal>(Action<TSignal> action)
        {
            _signalBus.Subscribe(action);
            _subscriptions.Add((typeof(TSignal), action));
        }
        
        protected void Subscribe<TSignal>(Action action)
        {
            _signalBus.Subscribe<TSignal>(action);
            _subscriptions.Add((typeof(TSignal), action));
        }
    
        protected void Unsubscribe<TSignal>(Action<TSignal> action)
        {
            _signalBus.TryUnsubscribe(action);
            _subscriptions.Remove((typeof(TSignal), action));
        }
        
        protected void UnsubscribeAll()
        {
            foreach (var (type, action) in _subscriptions)
            {
                _signalBus.TryUnsubscribe(type, action);
            }
            
            _subscriptions.Clear();
        }
        
        protected void FireSignal<TSignal>(TSignal signal)
        {
            _signalBus.Fire(signal);
        }
        
        protected void FireSignal<TSignal>()
        {
            _signalBus.Fire<TSignal>();
        }

        #endregion
        
        
        #region Controllers
        
        [Inject] private readonly DiContainer _container;
        private readonly List<BaseController> _controllers = new();
        
        protected BaseController CreateController(Type controllerType, params object[] args)
        {
            if (!typeof(BaseController).IsAssignableFrom(controllerType))
            {
                throw new ArgumentException($"Type {controllerType.Name} is not a BaseController");
            }
            
            var controller = (BaseController)_container.Instantiate(controllerType, args);
            AddController(controller);
            return controller;
        }
        
        protected T CreateController<T>(params object[] args)
        {
            var controller = _container.Instantiate<T>(args);
            AddController(controller as BaseController);
            return controller;
        }
        
        protected bool TryCreateControllerWithId<T>(object id, out T controller) where T : class
        {
            controller = _container.TryResolveId<T>(id);

            if (controller != null)
            {
                AddController(controller as BaseController);
            }

            return controller != null;
        }
        
        protected T CreateControllerWithId<T>(object id)
        {
            var controller = _container.ResolveId<T>(id);
            AddController(controller as BaseController);
            return controller;
        }
        
        protected void AddController(BaseController controller)
        {
            controller.Initialize();
            _controllers.Add(controller);
        }
        
        protected void RemoveController(BaseController controller, bool dispose = false)
        {
            if (_controllers.Remove(controller) && dispose)
            {
                controller.Dispose();
            }
        }
        
        protected void RemoveAllControllers(bool dispose = false)
        {
            while (_controllers.Count > 0)
            {
                RemoveController(_controllers[0], dispose);
            }
        }
        
        #endregion


        #region Coroutines

        [Inject] private readonly AsyncProcessor _asyncProcessor;
        private readonly Dictionary<int, CoroutineInfo> _coroutines = new();
        
        protected CoroutineInfo StartCoroutine(IEnumerator enumerator)
        {
            var coroutine = _asyncProcessor.Process(enumerator);
            _coroutines.Add(coroutine.Id, coroutine);
            return coroutine;
        }
        
        protected void StopCoroutine(CoroutineInfo coroutine)
        {
            if (coroutine == null || !_coroutines.ContainsKey(coroutine.Id))
            {
                return;
            }
            
            _asyncProcessor.Stop(coroutine);
            _coroutines.Remove(coroutine.Id);
        }
        
        protected void StopAllCoroutines()
        {
            _asyncProcessor.StopAll();
            _coroutines.Clear();
        }

        #endregion
    }
}