import * as esbuild from 'esbuild'
import { glob } from 'glob'
import pkg from './package.json' assert { type: 'json' }

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
const files = await glob(pkg.cssFiles);
await esbuild.build({
    entryPoints: files,
    loader: { '.css': 'css' },
    outfile: pkg.cssBundle,
    bundle: true,
    minify: true,
    sourcemap: false,
});
