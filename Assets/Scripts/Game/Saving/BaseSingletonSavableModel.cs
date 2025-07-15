using System;
using Zenject;

namespace Game.Saving
{
    [Serializable]
    public class BaseSingletonSavableModel : IInitializable
    {
        [Inject] private readonly SavingService _savingService;
        
        public void Initialize()
        {
            _savingService.AddSavableObject(this, GetType().Name);
        }
    }
}