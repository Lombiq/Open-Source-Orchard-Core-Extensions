const { FlatCompat } = require('@eslint/eslintrc');
const compat = new FlatCompat({ baseDirectory: __dirname });

const eslintrc = compat.config({
    // The following path may have to be adjusted to your directory structure.
    extends: './src/Utilities/Lombiq.NodeJs.Extensions/Lombiq.NodeJs.Extensions/config/.eslintrc.lombiq-base.js',

    // Add custom rules and overrides here.
    rules: {
    },
});

module.exports = [ ...eslintrc ]
