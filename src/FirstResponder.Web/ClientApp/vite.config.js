const path = require('path')
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

module.exports = defineConfig({
    plugins: [vue()],
    build: {
        outDir: path.resolve(__dirname, '../wwwroot/dist'),
        minify: 'terser',
        lib: {
            entry: path.resolve(__dirname, './src/main.js'),
            name: 'App',
            fileName: (format) => `js/app.${format}.js`,
            formats: ['umd']
        },
        rollupOptions: {
            external: ['vue'],
            output: {
                globals: {
                    vue: 'Vue'
                },
                assetFileNames: (chunkInfo) => {
                    if (chunkInfo.name === 'style.css')
                        return 'css/app.css'
                },
            }
        }
    }
})
