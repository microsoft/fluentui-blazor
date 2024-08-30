import * as esbuild from 'esbuild'
import { glob } from 'glob'
import { writeFile, unlink } from 'fs/promises';
import pkg from './package.json' with { type: 'json' }

// JS: Microsoft.FluentUI.AspNetCore.Components.lib.module.js
await esbuild.build({
    entryPoints: [pkg.source],
    bundle: true,
    minify: true,
    sourcemap: true,
    logLevel: 'info',
    target: 'es2022',
    format: 'esm',
    outfile: pkg.main,
    legalComments: 'none',
});

// CSS: Microsoft.FluentUI.AspNetCore.Components.bundle.scp.css
const allStyleFile = 'all-styles.css';
const files = await glob(pkg.cssFiles);
const content = files.map(file => `@import "${file.replace(/\\/g, '/')}";`).join('\n');

await writeFile(allStyleFile, content);
await esbuild.build({
    entryPoints: [allStyleFile],
    loader: { '.css': 'css' },
    outfile: pkg.cssBundle,
    bundle: true,
    minify: true,
    sourcemap: false,
});
await unlink(allStyleFile);
