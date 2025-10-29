import * as esbuild from 'esbuild'
import { pluginCompress } from '@espcom/esbuild-plugin-compress';
import pkg from './package.json' with { type: 'json' }

await esbuild.build({
    entryPoints: [ pkg.source ],
    bundle: true,
    write: false,
    minify: true,
    sourcemap: true,
    logLevel: 'info',
    target: 'es2022',
    format: 'esm',
    outfile: pkg.main,
    legalComments: 'external',
    plugins: [
        pluginCompress({
            gzip: true,  // Enable gzip compression
            brotli: true,  // Enable brotli compression
            zstd: true,  // Enable zstd compression
            level: 'high',  // Compression level: low, high, max
            extensions: ['.js']  // File extensions to compress
        })
    ],
});