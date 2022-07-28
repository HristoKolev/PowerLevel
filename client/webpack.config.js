const path = require('path');
const ESLintPlugin = require('eslint-webpack-plugin');
const ForkTsCheckerWebpackPlugin = require('fork-ts-checker-webpack-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const BrowserSyncPlugin = require('browser-sync-webpack-plugin');
const StylelintPlugin = require('stylelint-webpack-plugin');
const FaviconsWebpackPlugin = require('favicons-webpack-plugin');

const FaviconsWebpackFixerPlugin = require('./FaviconsWebpackFixerPlugin');

const pwaPlugins = [
  new FaviconsWebpackPlugin({
    logo: './src/logo.svg',
    mode: 'webapp',
    devMode: 'webapp',
    cache: true,
    inject: true,
    prefix: 'resources/[contenthash]/',
    manifest: './src/manifest.webmanifest',
    favicons: {
      icons: {
        coast: false,
        yandex: false,
        windows: false,
      },
    },
  }),
  new FaviconsWebpackFixerPlugin({
    prefixPath: 'resources/',
  }),
];

const styleLoader = {
  loader:
    process.env.NODE_ENV === 'development'
      ? 'style-loader'
      : MiniCssExtractPlugin.loader,
  options: {
    esModule: false,
  },
};

module.exports = {
  mode: process.env.NODE_ENV,
  context: __dirname,
  devtool: 'source-map',
  entry: {
    main: './src/index.tsx',
    ['service-worker']: './src/service-worker.ts',
  },
  output: {
    path: path.join(__dirname, 'dist'),
    publicPath: './',
    filename: ({ chunk: { name } }) => {
      return name === 'service-worker'
        ? '[name].js'
        : '[name].[contenthash].js';
    },
    assetModuleFilename: 'resources/[name].[contenthash][ext]',
  },
  module: {
    rules: [
      {
        test: /\.[jt]sx?$/,
        use: [
          { loader: 'babel-loader' },
          {
            loader: '@linaria/webpack-loader',
            options: {
              sourceMap: true,
            },
          },
        ],
        exclude: /node_modules/,
      },
      {
        test: /\.html$/,
        use: {
          loader: 'html-loader',
        },
      },
      {
        test: /\.(ico|gif|png|jpg|jpeg|svg|woff|woff2|eot|ttf|otf)$/i,
        type: 'asset',
      },
      {
        test: /\.css$/,
        use: [
          styleLoader,
          { loader: 'css-loader' },
          { loader: 'postcss-loader' },
        ],
      },
      {
        test: /\.scss$/,
        use: [
          styleLoader,
          { loader: 'css-loader' },
          { loader: 'postcss-loader' },
          {
            loader: 'resolve-url-loader',
            options: {
              sourceMap: true,
            },
          },
          {
            loader: 'sass-loader',
            options: {
              sourceMap: true,
              implementation: require('sass'),
            },
          },
        ],
      },
    ],
  },
  plugins: [
    new CleanWebpackPlugin(),
    new HtmlWebpackPlugin({
      filename: 'index.html',
      inject: 'body',
      template: 'src/index.html',
      excludeChunks: ['service-worker'],
    }),
    new ESLintPlugin({
      extensions: ['js', 'jsx', 'ts', 'tsx'],
      failOnWarning: true,
    }),
    new StylelintPlugin({
      failOnWarning: true,
    }),
    new ForkTsCheckerWebpackPlugin({
      typescript: {
        diagnosticOptions: {
          semantic: true,
          syntactic: true,
        },
      },
    }),
    ...(process.env.NODE_ENV === 'development'
      ? [
          new BrowserSyncPlugin(require('./browsersync-config')),
          ...(process.env.PWA_DEV === 'true' ? pwaPlugins : []),
        ]
      : []),
    ...(process.env.NODE_ENV === 'production'
      ? [
          new MiniCssExtractPlugin({
            filename: '[name].[contenthash].css',
            chunkFilename: '[id].[contenthash].css',
          }),
          ...pwaPlugins,
        ]
      : []),
  ],
  resolve: {
    extensions: ['.tsx', '.ts', '.jsx', '.js'],
    alias: {
      '@mui/base': '@mui/base/modern',
      '@mui/lab': '@mui/lab/modern',
      '@mui/material': '@mui/material/modern',
      '@mui/styled-engine': '@mui/styled-engine/modern',
      '@mui/system': '@mui/system/modern',
    },
  },
  stats: {
    errorDetails: true,
  },
  optimization: {
    splitChunks: {
      cacheGroups: {
        vendor: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendor',
          chunks: 'initial',
        },
      },
    },
  },
};
