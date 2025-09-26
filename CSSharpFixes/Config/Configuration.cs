using System.ComponentModel;
using System.Runtime.CompilerServices;
using CSSharpFixes.Managers;
using Microsoft.Extensions.Logging;

namespace CSSharpFixes.Config
{
    public class Configuration(ILogger<CSSharpFixes> logger, FixManager fixManager) : INotifyPropertyChanged
    {
        private bool enableWaterFix = true;
        private bool enableTriggerPushFix = false;
        private bool enableCPhysBoxUseFix = false;
        private bool enableNavmeshLookupLagFix = false;
        private bool enableNoBlock = false;
        private bool disableTeamMessages = false;
        private bool disableSubTickMovement = false;
        private bool enableMovementUnlocker = false;
        private bool enforceFullAlltalk = false;
        private bool enableHammerIDFix = false;
        private bool enableEmitSoundVolumeFix = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if(propertyName == null) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            OnConfigChanged(propertyName, GetType().GetProperty(propertyName)?.GetValue(this, null));
        }

        private void OnConfigChanged(string propertyName, object? newValue)
        {
            logger.LogInformation($"[CSSharpFixes][Configuration][OnConfigChanged()][Property={propertyName}][Value={newValue}]");
            fixManager.OnConfigChanged(propertyName, newValue);
        }
        
        public void Start()
        {
            logger.LogInformation("[CSSharpFixes][Configuration][Start()]");
            
            // Call OnConfigChanged for each property to apply the initial configuration & trigger the fix manager
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(this))
            {
                OnConfigChanged(property.Name, property.GetValue(this));
            }
        }

        public bool EnableWaterFix
        {
            get => enableWaterFix;
            set
            {
                if (enableWaterFix != value)
                {
                    enableWaterFix = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnableTriggerPushFix
        {
            get => enableTriggerPushFix;
            set
            {
                if (enableTriggerPushFix != value)
                {
                    enableTriggerPushFix = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool EnableCPhysBoxUseFix
        {
            get => enableCPhysBoxUseFix;
            set
            {
                if (enableCPhysBoxUseFix != value)
                {
                    enableCPhysBoxUseFix = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnableNavmeshLookupLagFix
        {
            get => enableNavmeshLookupLagFix;
            set
            {
                if (enableNavmeshLookupLagFix != value)
                {
                    enableNavmeshLookupLagFix = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnableNoBlock
        {
            get => enableNoBlock;
            set
            {
                if (enableNoBlock != value)
                {
                    enableNoBlock = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool DisableTeamMessages
        {
            get => disableTeamMessages;
            set
            {
                if (disableTeamMessages != value)
                {
                    disableTeamMessages = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool DisableSubTickMovement
        {
            get => disableSubTickMovement;
            set
            {
                if (disableSubTickMovement != value)
                {
                    disableSubTickMovement = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnableMovementUnlocker
        {
            get => enableMovementUnlocker;
            set
            {
                if (enableMovementUnlocker != value)
                {
                    enableMovementUnlocker = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnforceFullAlltalk
        {
            get => enforceFullAlltalk;
            set
            {
                if (enforceFullAlltalk != value)
                {
                    enforceFullAlltalk = value;
                    OnPropertyChanged();
                }
            }
        }

		public bool EnableHammerIDFix
		{
			get => enableHammerIDFix;
			set
			{
				if (enableHammerIDFix != value)
				{
					enableHammerIDFix = value;
					OnPropertyChanged();
				}
			}
		}

		public bool EnableEmitSoundVolumeFix
		{
			get => enableEmitSoundVolumeFix;
			set
			{
				if (enableEmitSoundVolumeFix != value)
				{
					enableEmitSoundVolumeFix = value;
					OnPropertyChanged();
				}
			}
		}
	}
}
