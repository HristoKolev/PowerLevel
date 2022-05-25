const path = require('path');
const PLUGIN_NAME = 'FaviconsWebpackFixerPlugin';

class FaviconsWebpackFixerPlugin {
  constructor(options = {}) {
    const opt = options || {};

    if (!opt.prefixPath) {
      throw new Error(
        `You must specify a 'prefixPath' option for ${PLUGIN_NAME}.`
      );
    }

    this.options = opt;
  }

  apply(compiler) {
    compiler.hooks.initialize.tap(PLUGIN_NAME, () => {
      compiler.hooks.make.tapPromise(PLUGIN_NAME, async (compilation) => {
        const HtmlWebpackPlugin = compiler.options.plugins
          .map(({ constructor }) => constructor)
          .find(
            (constructor) =>
              constructor && constructor.name === 'HtmlWebpackPlugin'
          );

        if (!HtmlWebpackPlugin) {
          throw new Error("Can't find an instance of HtmlWebpackPlugin.");
        }

        HtmlWebpackPlugin.getHooks(compilation).alterAssetTags.tapAsync(
          PLUGIN_NAME,
          (htmlPluginData, htmlWebpackPluginCallback) => {
            // Find the manifest meta tag.
            const manifestMeta = htmlPluginData.assetTags.meta.find(
              (x) => x.attributes.rel === 'manifest'
            );

            if (!manifestMeta) {
              throw new Error("Can't find a meta tag with rel=manifest.");
            }

            // Override it's attributes.
            manifestMeta.attributes.crossorigin = 'use-credentials';
            manifestMeta.attributes.href = 'manifest.webmanifest';

            htmlWebpackPluginCallback(null, htmlPluginData);
          }
        );
      });

      const { Compilation, sources } = compiler.webpack;
      const { RawSource } = sources;

      compiler.hooks.thisCompilation.tap(PLUGIN_NAME, (compilation) => {
        compilation.hooks.processAssets.tapPromise(
          {
            name: PLUGIN_NAME,
            stage: Compilation.PROCESS_ASSETS_STAGE_ADDITIONS,
          },
          async () => {
            const prefixPath = this.options.prefixPath
              .replaceAll('\\', path.sep)
              .replaceAll('/', path.sep);

            const resourceEntry = Object.entries(compilation.assets).find(
              ([sourcePath]) =>
                sourcePath.startsWith(prefixPath) &&
                sourcePath.endsWith('manifest.json')
            );

            if (!resourceEntry) {
              throw new Error(
                "Can't find the manifest.json asset in the webpack compilation."
              );
            }

            const [sourcePath, source] = resourceEntry;
            const basePath = path.dirname(sourcePath);

            const manifestObject = JSON.parse(source.source());
            for (const icon of manifestObject.icons) {
              icon.src = path.join(basePath, icon.src).replaceAll('\\', '/');
            }

            compilation.emitAsset(
              'manifest.webmanifest',
              new RawSource(JSON.stringify(manifestObject, null, 2), false)
            );

            compilation.deleteAsset(sourcePath);

            compilation.deleteAsset(path.join(basePath, 'manifest.webapp'));
          }
        );
      });
    });
  }
}

module.exports = FaviconsWebpackFixerPlugin;
