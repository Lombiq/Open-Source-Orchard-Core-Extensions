import { defineConfig } from "eslint/config";

// The following path may have to be adjusted to your directory structure.
import baseConfigs from './src/Utilities/Lombiq.NodeJs.Extensions/Lombiq.NodeJs.Extensions/config/eslint.config.lombiq-base.js';

export default defineConfig([{
    extends: baseConfigs,

    // Add custom rules and overrides here.
    rules: {
    },
}]);
