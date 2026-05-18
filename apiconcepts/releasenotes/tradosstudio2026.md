# Release Notes for Var:ProductNameWithEdition

As part of our ongoing efforts to simplify and streamline the Trados Studio development experience, we are revisiting the existing Trados Studio APIs. During this process, several classes, interfaces, and methods that were identified as redundant or unnecessarily complex have been removed.

Looking ahead, we plan to gradually phase out portions of the current API set and replace them with APIs aligned with the new Trados Studio architecture. This transition is intended to improve performance, provide a consistent developer experience, and simplify plugin integration. Our goal is to make this transition as smooth as possible, and we will provide ample notice whenever an API is deprecated and before it is eventually removed.

In addition, direct interaction with Trados GroupShare resources will be limited. For all server-based resource interactions, we strongly recommend using the GroupShare API Toolkit, which is designed specifically for secure and efficient integration with GroupShare services. You can find resources [here](https://developers.rws.com/groupshare-api-docs/apiconcepts/overview.html). 

We value feedback from our developer community. Share your thoughts, implementation experiences, and questions regarding these changes on the [Trados Studio Developers Forum](https://community.rws.com/developers-more/trados-portfolio/trados-studio-developers/f/sdk_qa). Your input will help refine the APIs and ensure a smoother transition to the new architecture.

You can find the complete set of changes in the [How to Upgrade section of the documentation](../../articles/hints_tips/Update_Plugins/how_to_update_plugins_to_trados_studio.md), which includes detailed information on the removed APIs, their replacements, and guidance on how to update your existing plugins. We encourage all developers to review this section carefully to understand the implications of these changes and to plan for necessary updates to their plugins.