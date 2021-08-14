import { SettingsService } from '../services/settings.service';

export function initSynchronousFactory() {
  return (settings: SettingsService) => {
    // run initialization of all settings that are stored in the local storage of the device
  };
}
