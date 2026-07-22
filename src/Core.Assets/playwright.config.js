import { defineConfig } from "@playwright/test";

export default defineConfig({
    outputDir: "obj/playwright",
    reporter: "line",
    testDir: "tests"
});