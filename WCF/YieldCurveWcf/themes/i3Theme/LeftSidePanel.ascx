<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeftSidePanel.ascx.cs" EnableViewState="false"
    Inherits="User_controls_LeftSidePanel" %>
<%@ Register Src="~/admin/menu.ascx" TagName="menu" TagPrefix="uc1" %>
<%@ Import Namespace="BlogEngine.Core" %>

<!--sidebox start -->
<div id="pages" class="dbx-box widget_rss">
    <h3 class="dbx-handle">
        DemoApps</h3>
    <div class="dbx-content">
        <blog:PageList ID="PageList1" runat="Server" />
    </div>
</div>
<!--sidebox end -->
<!--sidebox start -->
<div id="categories" class="dbx-box widget_categories">
    <h3 class="dbx-handle">
        <!--Categories</h3>-->
		Pages</h3>
    <div class="dbx-content">
        <blog:CategoryList ID="CategoryList1" runat="Server" />
    </div>
</div>
<!--sidebox end -->
<!--sidebox start -->
<div id="archives" class="dbx-box widget_archives">
	<h3 class="dbx-handle">
        Archives</h3>
    <div class="dbx-content">
        <blog:MonthList ID="MonthList" runat="Server" />
    </div>
</div>
<!--sidebox end -->
<!--sidebox start -->

<div id="disclaimer" class="dbx-box widget_rss">
    <h3 class="dbx-handle">
        Disclaimer</h3>
    <div class="dbx-content">
        Hello World!.
        <br />
        <br />
         Copyright 2011
    </div>
</div>

<!--sidebox end -->

<!-- admin start -->
<% if (Page.User.Identity.IsAuthenticated) { %>
<div id="Div1" class="dbx-box widget_categories">
    <h3 class="dbx-handle">
        Administration</h3>
    <div class="dbx-content">
        <uc1:menu ID="Menu1" runat="server" />
    </div>
</div>
<%} %>
<!-- admin end -->
