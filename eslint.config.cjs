const { defineConfig } = require('eslint/config');

// The following path may have to be adjusted to your directory structure.
const { baseConfigs } = require('./src/Utilities/Lombiq.NodeJs.Extensions/Lombiq.NodeJs.Extensions/config/.eslintrc.lombiq-base.js');

module.exports = defineConfig([{
    extends: baseConfigs,

    // Add custom rules and overrides here.
    rules: {
    },
}]);
