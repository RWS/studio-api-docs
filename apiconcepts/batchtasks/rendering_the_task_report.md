Rendering the Task Report
============================
Configure the report of your custom batch task.

How to render the Task Report XML with XSLT
-----------------------------------

Follow the next steps to add a report. During the application logic implementation, you already learned that you need to construct an XML string for the report content. When you call the **CreateReport()** method you pass the report content 
        XML to this method.

For <Var:ProductName> to render the report XML, you need to develop a matching XSL stylesheet and add it to your project and make sure that in your Visual Studio project file the stylesheet is included as an embedded resource:
# [XSLT Stylesheet](#tab/tabid-1)
[!code-xml[ReportXSLT](code_samples/Stylesheet.xsl)]		
***

When rendered in Studio, the report looks as shown below:
<img style="display:block; " src="images/ReportOutput.jpg" />