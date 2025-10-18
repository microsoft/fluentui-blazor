import path from 'path';
import { fileURLToPath } from 'url';
import CompressionPlugin from 'compression-webpack-plugin';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
/**
 * Webpack configuration for .esproj with Gzip and Brotli compression.
 */
export default {
  entry: './src/index.ts', // Adjust if your entry file differs
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'Microsoft.FluentUI.AspNetCore.Components.lib.module.js',
    clean: true,
  },
  resolve: {
    extensions: ['.ts', '.js'],
  },
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
    ],
  },
  plugins: [
    // Gzip compression
    new CompressionPlugin({
      filename: '[path][base].gz',
      algorithm: 'gzip',
      test: /\.(js|css|html|svg)$/,
      threshold: 10240,
      minRatio: 0.8,
    }),
    // Brotli compression
    new CompressionPlugin({
      filename: '[path][base].br',
      algorithm: 'brotliCompress',
      test: /\.(js|css|html|svg)$/,
      compressionOptions: { level: 11 },
      threshold: 10240,
      minRatio: 0.8,
    }),
  ],
  mode: 'production',
};
