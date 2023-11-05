import * as esbuild from 'esbuild'
import package from './package.json';

await esbuild.build({
    entryPoints: [ package.source ],
    bundle: true,
    minify: true,
    sourcemap: true,
    logLevel: 'info',
    target: 'es2022',
    format: 'esm',
    outfile: package.main,
});