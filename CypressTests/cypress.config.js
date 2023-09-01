const { defineConfig } = require('cypress')
const dotenv = require('dotenv')
const { generateZapReport } = require('./cypress/plugins/generateZapReport')

dotenv.config();

module.exports = defineConfig({
  video: false,
  e2e: {
    setupNodeEvents(on, config) {

      on('before:run', () => {
        // Map cypress env vars to process env vars for usage outside of Cypress run
        process.env = config.env
      })

      on('after:run', async () => {
        if(process.env.ZAP) {
          await generateZapReport()
        }
      })
    },
  },
});
