import { defineConfig } from "eslint/config";
import path from "node:path";
import { fileURLToPath } from "node:url";
import js from "@eslint/js";
import { FlatCompat } from "@eslint/eslintrc";

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const compat = new FlatCompat({
    baseDirectory: __dirname,
    recommendedConfig: js.configs.recommended,
    allConfig: js.configs.all
});

export default defineConfig([{
    // The following path may have to be adjusted to your directory structure.
    extends: compat.extends('./src/Utilities/Lombiq.NodeJs.Extensions/Lombiq.NodeJs.Extensions/config/.eslintrc.lombiq-base.js'),

    // Add custom rules and overrides here.
    rules: {
    },
}]);
