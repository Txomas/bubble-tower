using System;
using System.Collections.Generic;

namespace Zenject.Helpers
{
    public abstract class BaseController : IInitializable, ITickable, IDisposable
    {
        [Inject] private readonly DiContainer _container;
        [Inject] private readonly SignalBus _signalBus;
        private readonly List<(Type, object)> _subscriptions = new();
        private readonly List<BaseController> _controllers = new();
        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
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
            OnInitialized();
        
            foreach (var controller in _controllers)
            {
                controller.Initialize();
            }
        }

        public void Tick()
        {
            if (!IsEnabled)
            {
                return;
            }
            
            OnTick();
        
            foreach (var controller in _controllers)
            {
                controller.Tick();
            }
        }

        public void Dispose()
        {
            foreach (var (type, action) in _subscriptions)
            {
                _signalBus.TryUnsubscribe(type, action);
            }
        
            foreach (var controller in _controllers)
            {
                controller.Dispose();
            }
        
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
        
        protected void FireSignal<TSignal>(TSignal signal)
        {
            _signalBus.Fire(signal);
        }
        
        protected void FireSignal<TSignal>()
        {
            _signalBus.Fire<TSignal>();
        }
    
        protected T CreateController<T>(params object[] args) where T : BaseController
        {
            var controller = _container.Instantiate<T>(args);
            AddController(controller);
            return controller;
        }
        
        protected void AddController(BaseController controller)
        {
            controller.Initialize();
            _controllers.Add(controller);
        }
    }
}