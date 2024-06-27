const { defineConfig } = require('cypress')
const dotenv = require('dotenv')
const { generateZapReport } = require('./cypress/plugins/generateZapReport')

dotenv.config()

module.exports = defineConfig({
  env: {
    url: process.env.url,
    apiKey: process.env.apiKey,
    URN: process.env.URN,
  },
  video: false,
  userAgent: 'DfEAcademiesAcademisation/1.0 Cypress',
  e2e: {
    setupNodeEvents(on, config) {
      on('after:run', async () => {
        if (process.env.ZAP) {
          await generateZapReport()
        }
      })
    },
  },
})
