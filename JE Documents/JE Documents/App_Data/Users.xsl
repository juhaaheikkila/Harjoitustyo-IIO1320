<?xml version="1.0"?>
<xsl:stylesheet version="1.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <h2>Palautteet</h2>
    <table border="1">
      <tr bgcolor="#9acd32">
        <th>User id</th>
        <th>Username</th>
        <th>Department</th>
        <th>Email</th>
        <th>Roles</th>
      </tr>
      <xsl:for-each select="users/user">
        <tr>
          <td>
            <xsl:value-of select="id"/>
          </td>
          <td>
            <xsl:value-of select="name"/>
          </td>
          <td>
            <xsl:value-of select="department"/>
          </td>
          <td>
            <xsl:value-of select="email"/>
          </td>
          <td>
            <xsl:for-each select="roles">
              <xsl:value-of select="role"/>
              <br />
            </xsl:for-each>
          </td>
        </tr>
      </xsl:for-each>
    </table>
  </xsl:template>
</xsl:stylesheet>