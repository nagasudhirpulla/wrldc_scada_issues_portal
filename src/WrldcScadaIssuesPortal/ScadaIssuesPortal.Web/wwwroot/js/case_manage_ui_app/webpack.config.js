const path = require('path');

module.exports = {
    // multiple entry points - https://github.com/webpack/docs/wiki/multiple-entry-points
    entry: {
        case_edit_ui: ['babel-polyfill', path.resolve(__dirname, 'src/case_edit_ui.ts')]
    },

    output: {
        filename: 'bundle.[name].js'
    },

    // https://webpack.js.org/configuration/externals/
    externals: {
        "react": "React",
        "react-dom": "ReactDOM",
        jquery: 'jQuery'
    },

    // Enable sourcemaps for debugging webpack's output.
    devtool: "source-map",

    module: {
        rules: [
            {
                test: /\.ts$/,
                exclude: /node_modules/,
                use: ["babel-loader", "ts-loader"]
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: ["babel-loader"]
            },
            // All output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
            {
                enforce: "pre",
                test: /\.js$/,
                loader: "source-map-loader"
            }
        ]
    },

    plugins: [],

    resolve: {
        extensions: ['.js', '.ts'],
    }
};