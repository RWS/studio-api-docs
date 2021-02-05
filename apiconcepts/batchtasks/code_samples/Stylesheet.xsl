<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="html" indent="yes" encoding="utf-8"/> 
<xsl:template match="/"> 
	<html>
		<head>
			<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
			<style type="text/css">
        body, p { font-family: arial, helvetica;font-size:10pt }
        h1 { font-size: 14pt;font-weight:bold;color:navy}
        .fileName { font-weight: normal }
        .segmentStatus { font-weight: normal }
        .processTime { font-weight: normal }
        .language { font-weight: normal }
        .label { font-weight: bold }
      </style>
		</head>
		<body bgcolor="white">
      <h1>Task name: Sample Batch Task</h1>			
      <b>Segment status to export: </b> <xsl:value-of select="/report/@segmentStatus"/>
      <hr/>
      <xsl:apply-templates select="/report"/>
      <p/>
		</body>
	</html>		
</xsl:template> 
<!-- ***********************************************************************-->
<xsl:template match="file">
  <div>
    	<span class="label">File name: </span><span class="fileName"><xsl:value-of select="fileName"/></span><p/>
      <span class="label">Language: </span><span class="language"><xsl:value-of select="language"/></span><p/>
      <span class="label">Processed at: </span><span class="processTime"><xsl:value-of select="processTime"/></span><p/>
</div>
  <hr/>
</xsl:template>
<!-- ***********************************************************************-->
</xsl:stylesheet>