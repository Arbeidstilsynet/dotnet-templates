import { defineConfig } from "@hey-api/openapi-ts";

// Shared by every frontend; each one sets baseUrl and interceptors at startup.
export default defineConfig({
  input: "./generated/openApi.json",
  output: "./generated/api-client",
  plugins: [
    { name: "@hey-api/client-fetch", exportFromIndex: true, throwOnError: true },
    "@hey-api/typescript",
    "@hey-api/sdk",
  ],
});
