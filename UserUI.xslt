<?xml version="1.0"?>
<!--
Copyright (c) 2008 Corel Corporation.

Permission is hereby granted, free of charge, to any person or organization obtaining a copy of the software and accompanying 
documentation covered by this license (the "Software") to use, reproduce, display, distribute, execute, and transmit the Software, 
and to prepare derivative works of the Software, and to permit third-parties to whom the Software is furnished to do so, all subject 
to the following:

The copyright notices in the Software and this entire statement, including the above license grant, the original location it was 
downloaded from, this restriction and the following disclaimer, must be included in all copies of the Software, in whole or in part, 
and all derivative works of the Software, unless such copies or derivative works are solely in the form of machine-executable object
code generated by a source language processor.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. THE SOFTWARE MAY CONTAIN BUGS, ERRORS AND OTHER
PROBLEMS THAT COULD CAUSE SYSTEM FAILURES AND THE USE OF SUCH SOFTWARE IS ENTIRELY AT THE USER'S RISK. IN NO EVENT SHALL THE COPYRIGHT
HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE, 
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

************************************************************************************************************************************
This file adds controls to the current workspace.  It is only executed once per workspace (e.g. if you make changes, you must launch 
with F8 to reapply the changes.
************************************************************************************************************************************
-->
<xsl:stylesheet version="1.0"
								xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
								xmlns:frmwrk="Corel Framework Data"
								exclude-result-prefixes="frmwrk">
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>

  <!-- Use these elements for the framework to move the container from the app config file to the user config file -->
  <!-- Since these elements use the frmwrk name space, they will not be executed when the XSLT is applied to the user config file -->
  <frmwrk:uiconfig>
    <!-- The Application Info should always be the topmost frmwrk element -->
    <frmwrk:compositeNode xPath="/uiConfig/commandBars/commandBarData[@guid='3eaa9bbe-28fd-4672-9128-02974ee96332']"/>
    <frmwrk:compositeNode xPath="/uiConfig/frame"/>
  </frmwrk:uiconfig>

  <!-- Copy everything -->
  <xsl:template match="node()|@*">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
    </xsl:copy>
  </xsl:template>
	<xsl:template match="uiConfig/commandBars/commandBarData[@guid='f3016f3c-2847-4557-b61a-a2d05319cf18']/menubar/modeData[@guid='76d73481-9076-44c9-821c-52de9408cce2']/item[@guidRef='6c91d5ab-d648-4364-96fb-3e71bcfaf70d']">
		<xsl:copy-of select="."/>
		<item guidRef="9387393f-8a16-4ee5-9ef5-ef9f4f8eb5b9"/>
	</xsl:template>
  <!-- Put the new command at the end of the 'dockers' menu -->
  <xsl:template match="commandBarData[@guid='3eaa9bbe-28fd-4672-9128-02974ee96332']/menu">
    <xsl:copy>
      <xsl:apply-templates select="node()|@*"/>
      <!-- Make sure we don't read the menu item it it already exists -->
      <xsl:if test="not(./item[@guidRef='fa55bc80-fd19-4869-9188-d59bd5bcfc39'])">
				<item guidRef="fa55bc80-fd19-4869-9188-d59bd5bcfc39"/>
			</xsl:if>
    </xsl:copy>
  </xsl:template>
  
</xsl:stylesheet>