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

const searchRoot = process.argv.length > 2 ? process.argv[2] : '.';
const files = findRecursively(
    searchRoot,
    [/\.md$/i],
    [/^node_modules$/, /^\.git$/, /^obj$/, /^bin$/]);

try {
    const results = markdownlint.sync({ files });

    Object.keys(results).forEach((fileName) => {

        results[fileName].forEach((warning) => {

            const line = warning.lineNumber;
            const column = (Array.isArray(warning.errorRange) && !isNaN(warning.errorRange[0])) 
                ? warning.errorRange[0]
                : 1;
            const [code, name] = Array.isArray(warning.ruleNames) 
                ? warning.ruleNames 
                : ['WARN', 'unknown-warning'];

            let message = `${name ? name : code}: ${warning.ruleDescription.trim()}`;
            if (!message.endsWith('.')) message += '.';
            if (warning.fixInfo) message += ' An automatic fix is available.';
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
