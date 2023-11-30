import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";
import { fileURLToPath, URL } from "node:url";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    port: 8080,
    strictPort: true,
    host: "localhost",
    proxy: {
      "/api": {
        target: "http://127.0.0.1:3000/",
        changeOrigin: true,
      },
    },
  },
});
