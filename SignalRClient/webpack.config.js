const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const CleanWebpackPlugin = require('clean-webpack-plugin');

module.exports =  {
    devtool: "source-map",
    node: {
        fs: 'empty'
    },
    entry: [
        './src/index.tsx'
    ],
    devServer: {
        historyApiFallback: true
    },
    mode:"development",
    watch: true,
    module: {
        rules: [
            {
                test: /\.(jpg|png|jpeg|gif|ico|woff|svg|woff2|eot|otf|ttf|mp3)$/,
                use: 'file-loader',
            },
            { 
                test: /\.tsx?$/, loader: "awesome-typescript-loader" 
            },
            {
                test: /\.css$/,
                exclude: /node_modules/,
                use: [
                    'style-loader',
                    {
                        loader: 'typings-for-css-modules-loader',
                        options: {
                            modules: true,
                            namedExport: true,
                            camelCase: true,
                            localIdentName: '[name]__[local]_[hash:base64:5]'
                        }
                    }
                ]
            },
            {
                test: /(ReactToastify)\.css$/,
                use: ['style-loader', 'css-loader'],
            },
        ],
        
    },
    resolve: {
        extensions: [".ts", ".tsx", ".js", ".json", ".css"],
        alias:{
            assets: path.resolve(__dirname, '../assets'),
            src: path.resolve(__dirname, '../src'),
            components: path.resolve(__dirname, "../src/components")
        }
    },
    plugins: [
        new HtmlWebpackPlugin({
            filename: '../index.html',
            template: './index.html',
        }),
    ],
    output: {
        path: __dirname + "/dist",
        filename: 'bundle.js',
        publicPath: '',
    },
};