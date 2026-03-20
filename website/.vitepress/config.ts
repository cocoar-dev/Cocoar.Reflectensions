import { defineConfig } from 'vitepress'
import { withMermaid } from 'vitepress-plugin-mermaid'
import llmstxt from 'vitepress-plugin-llms'

export default withMermaid(
  defineConfig({
    title: 'Cocoar.Reflectensions',
    description: 'Advanced reflection utilities for .NET — Type parsing, fluent reflection extensions, smart type conversion, and dynamic invocation.',

    head: [
      ['link', { rel: 'icon', type: 'image/svg+xml', href: '/logo_light.svg' }],
      ['link', { rel: 'alternate', type: 'text/plain', href: '/llms.txt', title: 'LLM documentation (summary)' }],
      ['link', { rel: 'alternate', type: 'text/plain', href: '/llms-full.txt', title: 'LLM documentation (full)' }],
    ],

    vite: {
      plugins: [llmstxt({
        excludeUnnecessaryFiles: false,
        ignoreFiles: ['changelog.md'],
      })],
    },

    themeConfig: {
      logo: {
        light: '/logo_light.svg',
        dark: '/logo_dark.svg',
      },

      siteTitle: 'Cocoar.Reflectensions v1',

      nav: [
        { text: 'Guide', link: '/guide/getting-started' },
        { text: 'Reference', link: '/reference/api' },
        { text: 'Changelog', link: '/changelog' },
        { text: 'LLM Docs', link: '/llms-full.txt', target: '_blank' },
        { text: 'NuGet', link: 'https://www.nuget.org/packages/Cocoar.Reflectensions' },
      ],

      sidebar: {
        '/guide/': [
          {
            text: 'Introduction',
            items: [
              { text: 'Getting Started', link: '/guide/getting-started' },
              { text: 'Why Reflectensions?', link: '/guide/why-cocoar-reflectensions' },
            ],
          },
          {
            text: 'Type System',
            items: [
              { text: 'Type Parsing', link: '/guide/type-parsing' },
              { text: 'Type Extensions', link: '/guide/type-extensions' },
            ],
          },
          {
            text: 'Object Reflection',
            items: [
              { text: 'Reflection Wrapper', link: '/guide/object-reflection' },
              { text: 'Smart Type Conversion', link: '/guide/type-conversion' },
            ],
          },
          {
            text: 'Reflection Queries',
            items: [
              { text: 'Method Extensions', link: '/guide/method-extensions' },
              { text: 'Dynamic Invocation <span class="badge-adv" title="Advanced topic"></span>', link: '/guide/dynamic-invocation' },
              { text: 'Expandable Objects <span class="badge-adv" title="Advanced topic"></span>', link: '/guide/expandable-objects' },
            ],
          },
          {
            text: 'Utilities',
            items: [
              { text: 'String Extensions', link: '/guide/string-extensions' },
              { text: 'Utility Extensions', link: '/guide/utility-extensions' },
            ],
          },
          {
            text: 'Migration',
            items: [
              { text: 'From doob.Reflectensions', link: '/guide/migration' },
            ],
          },
        ],
        '/reference/': [
          {
            text: 'Reference',
            items: [
              { text: 'API Overview', link: '/reference/api' },
              { text: 'Packages', link: '/reference/packages' },
            ],
          },
        ],
      },

      socialLinks: [
        { icon: 'github', link: 'https://github.com/cocoar-dev/Cocoar.Reflectensions' },
      ],

      search: {
        provider: 'local',
      },

      footer: {
        message: 'Released under the Apache-2.0 License.',
        copyright: 'Copyright 2025-present Cocoar',
      },
    },

    mermaid: {},

    mermaidPlugin: {
      class: 'mermaid',
    },
  }),
)
