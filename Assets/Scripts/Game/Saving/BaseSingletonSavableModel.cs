using System;
using Zenject;

namespace Game.Saving
{
    [Serializable]
    public class BaseSingletonSavableModel : IInitializable
    {
        [Inject] private readonly SavingService _savingService;
        
        protected virtual SavingCategory SavingCategory => SavingCategory.Undefined;

        public void Initialize()
        {
            _savingService.AddSavableObject(this, GetType().Name, SavingCategory);
        }
    }
}