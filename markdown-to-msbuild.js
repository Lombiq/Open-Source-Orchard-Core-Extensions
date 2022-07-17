const fs = require('fs');
const markdownlint = require('markdownlint');
const path = require('path');

function findRecursively(startPath, includeFiles, excludeDirectories) {
    const results = [];

    function addIfInclude(parentPath, fileEntity) {
        if (includeFiles.some((include) => fileEntity.name.match(include))) {
            results.push(path.resolve(parentPath, fileEntity.name))
        }
    }

    function findInner(here) {
        const segments = here.split(/[\\\/]+/);
        const name = segments[segments.length - 1];

        if (excludeDirectories.some((exclude) => name.match(exclude))) return;

        fs.readdirSync(here, { withFileTypes: true }).forEach((child) =>
            child.isDirectory()
                ? findInner(path.resolve(here, child.name))
                : addIfInclude(here, child));
    }

    findInner(path.resolve(startPath));

    return results;
}

const files = findRecursively(
    process.argv.length > 2 ? process.argv[2] : '.',
    [/\.md$/i],
    [/^node_modules$/, /^\.git$/, /^obj$/, /^bin$/]);
const config = {
  default: true,
  MD013: false,
  MD033: {
    allowed_elements: [
      // A special element in GitHub to indicate a keyboard key. Other Markdown formatters that don't support it will
      // safely ignore the tags and render the content as inline text without adverse effects.
      'kbd', 
    ]
  }
};

try {
    const results = markdownlint.sync({ files, config });

    Object.keys(results).forEach((fileName) => {
        results[fileName].forEach((warning) => {
            const line = warning.lineNumber;
            const column = (Array.isArray(warning.errorRange) && !isNaN(warning.errorRange[0])) 
                ? warning.errorRange[0]
                : 1;
            const [code, name] = Array.isArray(warning.ruleNames) 
                ? warning.ruleNames 
                : ['WARN', 'unknown-warning'];

            // License files don't need title.
            if (fileName.toLowerCase().endsWith('license.md') && code === 'MD041') return;

            let message = `${name ? name : code}: ${warning.ruleDescription.trim()}`;
            if (!message.endsWith('.')) message += '.';
            if (warning.fixInfo) message += ' An automatic fix is available with markdownlint-cli.';
            if (warning.ruleInformation) message += ' Rule information: ' + warning.ruleInformation;

            console.log(`${fileName}(${line},${column}): warning ${code}: ${message}`);
        });
    });
}
catch (error) {
    const code = error.code ? error.code : 'ERROR';
    console.log(`${error.path}(1,1): error ${code}: ${error.toString()}`);
    process.exit(1);
}
