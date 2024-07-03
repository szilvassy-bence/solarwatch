import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
      proxy: {
      '/api': {
        // eslint-disable-next-line no-undef
        target: process.env.VITE_BACKEND_URL || 'https://localhost:44376',
        changeOrigin: true,
      },
    },  
    /*proxy: {
      "/api": {
        target: "https://localhost:44376",
        changeOrigin: true,
        secure: false,
        ws: true,
      },
    },*/
  },
});
