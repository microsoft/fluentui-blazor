import * as esbuild from 'esbuild'
import pkg from './package.json' assert { type: 'json' }

await esbuild.build({
    entryPoints: [ pkg.source ],
    bundle: true,
    minify: true,
    sourcemap: true,
    logLevel: 'info',
    target: 'es2022',
    format: 'esm',
    outfile: pkg.main,
    legalComments: 'external',
});