using DotVVM.Framework.Binding;
using DotVVM.Framework.Binding.Expressions;
using DotVVM.Framework.Hosting;

namespace DotVVM.Framework.Controls
{
    /// <summary>
    /// Renders the pagination control which can be integrated with the GridViewDataSet object to provide the paging capabilities.
    /// </summary>
    [ControlMarkupOptions(AllowContent = false)]
    public class BootstrapDataPager : HtmlGenericControl
    {
        private static readonly CommandBindingExpression GoToNextPageCommand =
            new CommandBindingExpression(h => ((IPageableGridViewDataSet)h[0]).GoToNextPage(), "__$BootstrapDataPager_GoToNextPage");
        private static readonly CommandBindingExpression GoToThisPageCommand =
            new CommandBindingExpression(h => ((IPageableGridViewDataSet)h[1]).GoToPage((int)h[0]), "__$BootstrapDataPager_GoToThisPage");
        private static readonly CommandBindingExpression GoToPrevPageCommand =
            new CommandBindingExpression(h => ((IPageableGridViewDataSet)h[0]).GoToPreviousPage(), "__$BootstrapDataPager_GoToPrevPage");
        private static readonly CommandBindingExpression GoToFirstPageCommand =
            new CommandBindingExpression(h => ((IPageableGridViewDataSet)h[0]).GoToFirstPage(), "__$BootstrapDataPager_GoToFirstPage");
        private static readonly CommandBindingExpression GoToLastPageCommand =
            new CommandBindingExpression(h => ((IPageableGridViewDataSet)h[0]).GoToLastPage(), "__$BootstrapDataPager_GoToLastPage");


        /// <summary>
        /// Gets or sets the GridViewDataSet object in the viewmodel.
        /// </summary>
        [MarkupOptions(AllowHardCodedValue = false)]
        public IPageableGridViewDataSet DataSet
        {
            get { return (IPageableGridViewDataSet)GetValue(DataSetProperty); }
            set { SetValue(DataSetProperty, value); }
        }
        public static readonly DotvvmProperty DataSetProperty =
            DotvvmProperty.Register<IPageableGridViewDataSet, BootstrapDataPager>(c => c.DataSet);


        /// <summary>
        /// Gets or sets the template of the button which moves the user to the first page.
        /// </summary>
        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
        public ITemplate FirstPageTemplate
        {
            get { return (ITemplate)GetValue(FirstPageTemplateProperty); }
            set { SetValue(FirstPageTemplateProperty, value); }
        }
        public static readonly DotvvmProperty FirstPageTemplateProperty =
            DotvvmProperty.Register<ITemplate, BootstrapDataPager>(c => c.FirstPageTemplate, null);

        /// <summary>
        /// Gets or sets the template of the button which moves the user to the last page.
        /// </summary>
        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
        public ITemplate LastPageTemplate
        {
            get { return (ITemplate)GetValue(LastPageTemplateProperty); }
            set { SetValue(LastPageTemplateProperty, value); }
        }
        public static readonly DotvvmProperty LastPageTemplateProperty =
            DotvvmProperty.Register<ITemplate, BootstrapDataPager>(c => c.LastPageTemplate, null);

        /// <summary>
        /// Gets or sets the template of the button which moves the user to the previous page.
        /// </summary>
        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
        public ITemplate PreviousPageTemplate
        {
            get { return (ITemplate)GetValue(PreviousPageTemplateProperty); }
            set { SetValue(PreviousPageTemplateProperty, value); }
        }
        public static readonly DotvvmProperty PreviousPageTemplateProperty =
            DotvvmProperty.Register<ITemplate, BootstrapDataPager>(c => c.PreviousPageTemplate, null);

        /// <summary>
        /// Gets or sets the template of the button which moves the user to the next page.
        /// </summary>
        [MarkupOptions(AllowBinding = false, MappingMode = MappingMode.InnerElement)]
        public ITemplate NextPageTemplate
        {
            get { return (ITemplate)GetValue(NextPageTemplateProperty); }
            set { SetValue(NextPageTemplateProperty, value); }
        }
        public static readonly DotvvmProperty NextPageTemplateProperty =
            DotvvmProperty.Register<ITemplate, BootstrapDataPager>(c => c.NextPageTemplate, null);

        /// <summary>
        /// Gets or sets whether a hyperlink should be rendered for the current page number. If set to false, only a plain text is rendered.
        /// </summary>
        [MarkupOptions(AllowBinding = false)]
        public bool RenderLinkForCurrentPage
        {
            get { return (bool)GetValue(RenderLinkForCurrentPageProperty); }
            set { SetValue(RenderLinkForCurrentPageProperty, value); }
        }
        public static readonly DotvvmProperty RenderLinkForCurrentPageProperty =
            DotvvmProperty.Register<bool, BootstrapDataPager>(c => c.RenderLinkForCurrentPage);


        /// <summary>
        /// Gets or sets whether the pager should hide automatically when there is only one page of results. Must not be set to true when using the Visible property.
        /// </summary>
        [MarkupOptions(AllowBinding = false)]
        public bool HideWhenOnlyOnePage
        {
            get { return (bool)GetValue(HideWhenOnlyOnePageProperty); }
            set { SetValue(HideWhenOnlyOnePageProperty, value); }
        }
        public static readonly DotvvmProperty HideWhenOnlyOnePageProperty
            = DotvvmProperty.Register<bool, BootstrapDataPager>(c => c.HideWhenOnlyOnePage, true);

        public bool Enabled
        {
            get { return (bool)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }
        public static readonly DotvvmProperty EnabledProperty
            = DotvvmProperty.Register<bool, BootstrapDataPager>(c => c.Enabled, true);

        private HtmlGenericControl nav;
        private HtmlGenericControl firstLi;
        private HtmlGenericControl previousLi;
        private PlaceHolder numbersPlaceHolder;
        private HtmlGenericControl nextLi;
        private HtmlGenericControl lastLi;


        public BootstrapDataPager() : base("ul")
        {
        }

        protected override void OnLoad(Hosting.IDotvvmRequestContext context)
        {
            DataBind(context);
            base.OnLoad(context);
        }

        protected override void OnPreRender(Hosting.IDotvvmRequestContext context)
        {
            DataBind(context);
            base.OnPreRender(context);
        }

        private void CallGridViewDataSetRefreshRequest(IRefreshableGridViewDataSet gridViewDataSet)
        {
            gridViewDataSet.RequestRefresh();
        }
        private void DataBind(Hosting.IDotvvmRequestContext context)
        {
            if (DataSet is IRefreshableGridViewDataSet refreshableDataSet)
            {
                CallGridViewDataSetRefreshRequest(refreshableDataSet);
            }

            Children.Clear();

            nav = new HtmlGenericControl("ul");
            nav.SetBinding(DataContextProperty, GetDataSetBinding());

            var dataSet = DataSet;
            if (dataSet != null)
            {
                var enabledValue = HasValueBinding(EnabledProperty) ? (object)new ValueBindingExpression(h => GetValueBinding(EnabledProperty).Evaluate(this, EnabledProperty), "$pagerEnabled") : Enabled;

                // first button
                firstLi = new HtmlGenericControl("li");
                firstLi.Attributes.Add("class", "page-item");
                var firstLink = new LinkButton();
                firstLink.Attributes.Add("class", "page-link");
                SetButtonContent(context, firstLink, "««", FirstPageTemplate);
                firstLink.SetBinding(ButtonBase.ClickProperty, GoToFirstPageCommand);
                if (!true.Equals(enabledValue)) firstLink.SetValue(LinkButton.EnabledProperty, enabledValue);
                firstLi.Children.Add(firstLink);
                nav.Children.Add(firstLi);

                // previous button
                previousLi = new HtmlGenericControl("li");
                previousLi.Attributes.Add("class", "page-item");
                var previousLink = new LinkButton();
                previousLink.Attributes.Add("class", "page-link");
                SetButtonContent(context, previousLink, "«", PreviousPageTemplate);
                previousLink.SetBinding(ButtonBase.ClickProperty, GoToPrevPageCommand);
                if (!true.Equals(enabledValue)) previousLink.SetValue(LinkButton.EnabledProperty, enabledValue);
                previousLi.Children.Add(previousLink);
                nav.Children.Add(previousLi);

                // number fields
                numbersPlaceHolder = new PlaceHolder();
                nav.Children.Add(numbersPlaceHolder);

                var i = 0;
                foreach (var number in dataSet.PagingOptions.NearPageIndexes)
                {
                    var li = new HtmlGenericControl("li");
                    li.Attributes.Add("class", "page-item");
                    li.SetBinding(DataContextProperty, GetNearIndexesBinding(i));
                    if (number == dataSet.PagingOptions.PageIndex)
                    {
                        li.Attributes["class"] += " active";
                    }
                    var link = new LinkButton() { Text = (number + 1).ToString() };
                    link.Attributes["class"] = "page-link";
                    link.SetBinding(ButtonBase.ClickProperty, GoToThisPageCommand);
                    if (!true.Equals(enabledValue)) link.SetValue(LinkButton.EnabledProperty, enabledValue);
                    li.Children.Add(link);

                    numbersPlaceHolder.Children.Add(li);

                    i++;
                }

                // next button
                nextLi = new HtmlGenericControl("li");
                nextLi.Attributes.Add("class", "page-item");
                var nextLink = new LinkButton();
                nextLink.Attributes.Add("class", "page-link");
                SetButtonContent(context, nextLink, "»", NextPageTemplate);
                nextLink.SetBinding(ButtonBase.ClickProperty, GoToNextPageCommand);
                if (!true.Equals(enabledValue)) nextLink.SetValue(LinkButton.EnabledProperty, enabledValue);
                nextLi.Children.Add(nextLink);
                nav.Children.Add(nextLi);

                // last button
                lastLi = new HtmlGenericControl("li");
                lastLi.Attributes.Add("class", "page-item");
                var lastLink = new LinkButton();
                lastLink.Attributes.Add("class", "page-link");
                SetButtonContent(context, lastLink, "»»", LastPageTemplate);
                if (!true.Equals(enabledValue)) lastLink.SetValue(LinkButton.EnabledProperty, enabledValue);
                lastLink.SetBinding(ButtonBase.ClickProperty, GoToLastPageCommand);
                lastLi.Children.Add(lastLink);
                nav.Children.Add(lastLi);

                Children.Add(nav);
            }
        }

        private void SetButtonContent(Hosting.IDotvvmRequestContext context, LinkButton button, string text, ITemplate contentTemplate)
        {
            if (contentTemplate != null)
            {
                contentTemplate.BuildContent(context, button);
            }
            else
            {
                button.Text = text;
            }
        }

        private ValueBindingExpression GetNearIndexesBinding(int i)
        {
            return new ValueBindingExpression(
                        (h, c) => ((IPageableGridViewDataSet)h[0]).PagingOptions.NearPageIndexes[i],
                        "PagingOptions().NearPageIndexes[" + i + "]");
        }

        protected override void AddAttributesToRender(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            if (RenderOnServer)
            {
                throw new DotvvmControlException(this, "The BootstrapDataPager control cannot be rendered in the RenderSettings.Mode='Server'.");
            }
            
            base.AddAttributesToRender(writer, context);
        }

        protected override void AddVisibleAttributeOrBinding(IHtmlWriter writer)
        {
            if (!IsPropertySet(VisibleProperty))
            {
                if (HideWhenOnlyOnePage)
                {
                    writer.AddKnockoutDataBind("visible", $"ko.unwrap({GetDataSetBinding().GetKnockoutBindingExpression()}).PagingOptions().PagesCount() > 1");
                }
                else
                {
                    writer.AddKnockoutDataBind("visible", this, VisibleProperty, renderEvenInServerRenderingMode: true);
                }
            }
        }

        protected override void RenderBeginTag(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            if (HasValueBinding(EnabledProperty))
                writer.WriteKnockoutDataBindComment("dotvvm_introduceAlias", $"{{ '$pagerEnabled': { GetValueBinding(EnabledProperty).GetKnockoutBindingExpression() }}}");

            writer.AddKnockoutDataBind("with", this, DataSetProperty, renderEvenInServerRenderingMode: true);
            writer.AddAttribute("class", "pagination justify-content-center pb-2");
            writer.RenderBeginTag("ul");
        }

        protected override void RenderContents(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.AddKnockoutDataBind("css", "{ 'disabled': PagingOptions().IsFirstPage() }");
            firstLi.Render(writer, context);

            writer.AddKnockoutDataBind("css", "{ 'disabled': PagingOptions().IsFirstPage() }");
            previousLi.Render(writer, context);

            // render template
            writer.WriteKnockoutForeachComment("PagingOptions().NearPageIndexes");

            // render page number
            numbersPlaceHolder.Children.Clear();
            HtmlGenericControl li;
            if (!RenderLinkForCurrentPage)
            {
                writer.AddKnockoutDataBind("visible", "$data == $parent.PagingOptions().PageIndex()");
                writer.AddKnockoutDataBind("css", "{'active': $data == $parent.PagingOptions().PageIndex()}");
                li = new HtmlGenericControl("li");
                li.Attributes.Add("class", "page-item");
                var literal = new Literal();
                literal.Attributes.Add("class", "page-link");
                literal.DataContext = 0;
                literal.SetBinding(Literal.TextProperty, new ValueBindingExpression(vm => ((int)vm[0] + 1).ToString(), "$data + 1"));
                li.Children.Add(literal);
                numbersPlaceHolder.Children.Add(li);
                li.Render(writer, context);

                writer.AddKnockoutDataBind("visible", "$data != $parent.PagingOptions().PageIndex()");
            }
            writer.AddKnockoutDataBind("css", "{ 'active': $data == $parent.PagingOptions().PageIndex()}");
            li = new HtmlGenericControl("li");
            li.Attributes.Add("class", "page-item");
            li.SetValue(Internal.PathFragmentProperty, "PagingOptions().NearPageIndexes[$index]");
            var link = new LinkButton();
            link.Attributes.Add("class", "page-link");
            li.Children.Add(link);
            link.SetBinding(ButtonBase.TextProperty, new ValueBindingExpression(vm => ((int)vm[0] + 1).ToString(), "$data + 1"));
            link.SetBinding(ButtonBase.ClickProperty, GoToThisPageCommand);
            var enabledValue = HasValueBinding(EnabledProperty) ? (object)new ValueBindingExpression(h => GetValueBinding(EnabledProperty).Evaluate(this, EnabledProperty), "$pagerEnabled") : Enabled;
            if (!true.Equals(enabledValue)) link.SetValue(LinkButton.EnabledProperty, enabledValue);
            numbersPlaceHolder.Children.Add(li);
            li.Render(writer, context);

            writer.WriteKnockoutDataBindEndComment();

            writer.AddKnockoutDataBind("css", "{ 'disabled': PagingOptions().IsLastPage() }");
            nextLi.Render(writer, context);

            writer.AddKnockoutDataBind("css", "{ 'disabled': PagingOptions().IsLastPage() }");
            lastLi.Render(writer, context);
        }


        protected override void RenderEndTag(IHtmlWriter writer, IDotvvmRequestContext context)
        {
            writer.RenderEndTag();
            if (HasValueBinding(EnabledProperty)) writer.WriteKnockoutDataBindEndComment();
        }

        private IValueBinding GetDataSetBinding()
        {
            var binding = GetValueBinding(DataSetProperty);
            if (binding == null)
            {
                throw new DotvvmControlException(this, "The DataSet property of the dot:BootstrapDataPager control must be set!");
            }
            return binding;
        }
    }

}
