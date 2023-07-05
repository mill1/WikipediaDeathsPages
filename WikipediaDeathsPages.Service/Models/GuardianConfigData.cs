namespace WikipediaDeathsPages.Service.Models
{

    public class GuardianConfigData
    {
        public Config config { get; set; }
        public bool polyfilled { get; set; }
        public Adblockers adBlockers { get; set; }
        public Modules modules { get; set; }
        public Gadata GAData { get; set; }
        public Borkwebvitals borkWebVitals { get; set; }
    }

    public class Config
    {
        public bool isDotcomRendering { get; set; }
        public bool isDev { get; set; }
        public string stage { get; set; }
        public string frontendAssetsFullURL { get; set; }
        public Page page { get; set; }
        public Libs libs { get; set; }
        public Switches1 switches { get; set; }
        public Tests tests { get; set; }
        public Ophan ophan { get; set; }
    }

    public class Page
    {
        public string avatarApiUrl { get; set; }
        public Reference[] references { get; set; }
        public bool isProd { get; set; }
        public string shortUrlId { get; set; }
        public bool hasYouTubeAtom { get; set; }
        public Switches switches { get; set; }
        public int inBodyInternalLinkCount { get; set; }
        public string keywordIds { get; set; }
        public string blogIds { get; set; }
        public Sharedadtargeting sharedAdTargeting { get; set; }
        public string beaconUrl { get; set; }
        public object[] campaigns { get; set; }
        public string calloutsUrl { get; set; }
        public bool requiresMembershipAccess { get; set; }
        public string onwardWebSocket { get; set; }
        public bool hasMultipleVideosInPage { get; set; }
        public Pbindexsite[] pbIndexSites { get; set; }
        public string a9PublisherId { get; set; }
        public string toneIds { get; set; }
        public string dcrSentryDsn { get; set; }
        public string idWebAppUrl { get; set; }
        public string discussionApiUrl { get; set; }
        public string sentryPublicApiKey { get; set; }
        public string omnitureAccount { get; set; }
        public string contributorBio { get; set; }
        public string pageCode { get; set; }
        public string pillar { get; set; }
        public string commercialBundleUrl { get; set; }
        public string discussionApiClientHeader { get; set; }
        public string membershipUrl { get; set; }
        public string cardStyle { get; set; }
        public string sentryHost { get; set; }
        public bool shouldHideAdverts { get; set; }
        public bool shouldHideReaderRevenue { get; set; }
        public bool isPreview { get; set; }
        public string membershipAccess { get; set; }
        public string googletagJsUrl { get; set; }
        public string supportUrl { get; set; }
        public bool hasShowcaseMainElement { get; set; }
        public bool isColumn { get; set; }
        public bool isPaidContent { get; set; }
        public string sectionName { get; set; }
        public string mobileAppsAdUnitRoot { get; set; }
        public string dfpAdUnitRoot { get; set; }
        public string headline { get; set; }
        public bool commentable { get; set; }
        public string idApiUrl { get; set; }
        public bool showRelatedContent { get; set; }
        public string commissioningDesks { get; set; }
        public int inBodyExternalLinkCount { get; set; }
        public string adUnit { get; set; }
        public string stripePublicToken { get; set; }
        public string googleRecaptchaSiteKey { get; set; }
        public int videoDuration { get; set; }
        public string stage { get; set; }
        public string idOAuthUrl { get; set; }
        public bool isSensitive { get; set; }
        public string richLink { get; set; }
        public bool isDev { get; set; }
        public string thirdPartyAppsAccount { get; set; }
        public string avatarImagesUrl { get; set; }
        public string fbAppId { get; set; }
        public string externalEmbedHost { get; set; }
        public string ajaxUrl { get; set; }
        public string keywords { get; set; }
        public string revisionNumber { get; set; }
        public string blogs { get; set; }
        public string section { get; set; }
        public bool hasInlineMerchandise { get; set; }
        public string locationapiurl { get; set; }
        public string buildNumber { get; set; }
        public bool isPhotoEssay { get; set; }
        public string ampIframeUrl { get; set; }
        public string userAttributesApiUrl { get; set; }
        public bool isLive { get; set; }
        public string publication { get; set; }
        public string brazeApiKey { get; set; }
        public string host { get; set; }
        public string contentType { get; set; }
        public string facebookIaAdUnitRoot { get; set; }
        public string ophanEmbedJsUrl { get; set; }
        public string idUrl { get; set; }
        public bool thumbnail { get; set; }
        public int wordCount { get; set; }
        public bool isFront { get; set; }
        public string author { get; set; }
        public string nonKeywordTagIds { get; set; }
        public string dfpAccountId { get; set; }
        public string pageId { get; set; }
        public bool isAdFree { get; set; }
        public string forecastsapiurl { get; set; }
        public string assetsPath { get; set; }
        public Lightboximages lightboxImages { get; set; }
        public bool isImmersive { get; set; }
        public string dfpHost { get; set; }
        public string googletagUrl { get; set; }
        public string mmaUrl { get; set; }
        public Abtests abTests { get; set; }
        public string shortUrl { get; set; }
        public bool isContent { get; set; }
        public string contentId { get; set; }
        public string edition { get; set; }
        public string discussionFrontendUrl { get; set; }
        public string ipsosTag { get; set; }
        public string ophanJsUrl { get; set; }
        public string productionOffice { get; set; }
        public string plistaPublicApiKey { get; set; }
        public string tones { get; set; }
        public bool isLiveBlog { get; set; }
        public string frontendAssetsFullURL { get; set; }
        public string googleSearchId { get; set; }
        public bool allowUserGeneratedContent { get; set; }
        public string byline { get; set; }
        public string authorIds { get; set; }
        public long webPublicationDate { get; set; }
        public string omnitureAmpAccount { get; set; }
        public bool isHosted { get; set; }
        public bool hasPageSkin { get; set; }
        public string webTitle { get; set; }
        public string discussionD2Uid { get; set; }
        public string weatherapiurl { get; set; }
        public string googleSearchUrl { get; set; }
        public string optimizeEpicUrl { get; set; }
        public bool isSplash { get; set; }
        public bool isNumberedList { get; set; }
        public bool dcrCouldRender { get; set; }
    }

    public class Switches
    {
        public bool prebidAppnexusUkRow { get; set; }
        public bool mobileStickyPrebid { get; set; }
        public bool newsletterOnwards { get; set; }
        public bool abSignInGateMainVariant { get; set; }
        public bool commercialMetrics { get; set; }
        public bool prebidTrustx { get; set; }
        public bool scAdFreeBanner { get; set; }
        public bool abSignInGateCopyTestJan2023 { get; set; }
        public bool frontsBannerAds { get; set; }
        public bool prebidPermutiveAudience { get; set; }
        public bool compareVariantDecision { get; set; }
        public bool adFreeStrictExpiryEnforcement { get; set; }
        public bool enableSentryReporting { get; set; }
        public bool lazyLoadContainers { get; set; }
        public bool ampArticleSwitch { get; set; }
        public bool remarketing { get; set; }
        public bool keyEventsCarousel { get; set; }
        public bool verticalVideo { get; set; }
        public bool registerWithPhone { get; set; }
        public bool targeting { get; set; }
        public bool remoteHeader { get; set; }
        public bool deeplyReadSwitch { get; set; }
        public bool slotBodyEnd { get; set; }
        public bool prebidImproveDigitalSkins { get; set; }
        public bool ampPrebidOzone { get; set; }
        public bool extendedMostPopularFronts { get; set; }
        public bool emailInlineInFooter { get; set; }
        public bool showNewPrivacyWordingOnEmailSignupEmbeds { get; set; }
        public bool dcrNetworkFronts { get; set; }
        public bool iasAdTargeting { get; set; }
        public bool prebidAnalytics { get; set; }
        public bool extendedMostPopular { get; set; }
        public bool ampContentAbTesting { get; set; }
        public bool prebidCriteo { get; set; }
        public bool okta { get; set; }
        public bool abDeeplyReadArticleFooter { get; set; }
        public bool puzzlesBanner { get; set; }
        public bool imrWorldwide { get; set; }
        public bool acast { get; set; }
        public bool automaticFilters { get; set; }
        public bool twitterUwt { get; set; }
        public bool prebidAppnexusInvcode { get; set; }
        public bool ampPrebidPubmatic { get; set; }
        public bool a9HeaderBidding { get; set; }
        public bool prebidAppnexus { get; set; }
        public bool enableDiscussionSwitch { get; set; }
        public bool prebidXaxis { get; set; }
        public bool stickyVideos { get; set; }
        public bool interactiveFullHeaderSwitch { get; set; }
        public bool discussionAllPageSize { get; set; }
        public bool prebidUserSync { get; set; }
        public bool audioOnwardJourneySwitch { get; set; }
        public bool brazeTaylorReport { get; set; }
        public bool abConsentlessAds { get; set; }
        public bool externalVideoEmbeds { get; set; }
        public bool simpleReach { get; set; }
        public bool abIntegrateIma { get; set; }
        public bool callouts { get; set; }
        public bool carrotTrafficDriver { get; set; }
        public bool sentinelLogger { get; set; }
        public bool geoMostPopular { get; set; }
        public bool weAreHiring { get; set; }
        public bool relatedContent { get; set; }
        public bool thirdPartyEmbedTracking { get; set; }
        public bool prebidOzone { get; set; }
        public bool ampLiveblogSwitch { get; set; }
        public bool ampAmazon { get; set; }
        public bool borkFid { get; set; }
        public bool prebidAdYouLike { get; set; }
        public bool mostViewedFronts { get; set; }
        public bool optOutAdvertising { get; set; }
        public bool abSignInGateMainControl { get; set; }
        public bool headerTopNav { get; set; }
        public bool googleSearch { get; set; }
        public bool brazeSwitch { get; set; }
        public bool consentManagement { get; set; }
        public bool borkFcp { get; set; }
        public bool commercial { get; set; }
        public bool personaliseSignInGateAfterCheckout { get; set; }
        public bool redplanetForAus { get; set; }
        public bool prebidSonobi { get; set; }
        public bool idProfileNavigation { get; set; }
        public bool confiantAdVerification { get; set; }
        public bool discussionAllowAnonymousRecommendsSwitch { get; set; }
        public bool verticalVideoContainer { get; set; }
        public bool permutive { get; set; }
        public bool comscore { get; set; }
        public bool headerTopBarSearchCapi { get; set; }
        public bool ampPrebidCriteo { get; set; }
        public bool webFonts { get; set; }
        public bool europeNetworkFront { get; set; }
        public bool abBillboardsInMerch { get; set; }
        public bool prebidImproveDigital { get; set; }
        public bool offerHttp3 { get; set; }
        public bool ophan { get; set; }
        public bool crosswordSvgThumbnails { get; set; }
        public bool prebidTriplelift { get; set; }
        public bool weather { get; set; }
        public bool commercialOutbrainNewids { get; set; }
        public bool disableAmpTest { get; set; }
        public bool abLimitInlineMerch { get; set; }
        public bool serverShareCounts { get; set; }
        public bool abAdblockAsk { get; set; }
        public bool prebidPubmatic { get; set; }
        public bool autoRefresh { get; set; }
        public bool enhanceTweets { get; set; }
        public bool prebidIndexExchange { get; set; }
        public bool prebidOpenx { get; set; }
        public bool abElementsManager { get; set; }
        public bool prebidHeaderBidding { get; set; }
        public bool idCookieRefresh { get; set; }
        public bool serverSideLiveblogInlineAds { get; set; }
        public bool discussionPageSize { get; set; }
        public bool smartAppBanner { get; set; }
        public bool boostGaUserTimingFidelity { get; set; }
        public bool historyTags { get; set; }
        public bool mobileStickyLeaderboard { get; set; }
        public bool brazeContentCards { get; set; }
        public bool surveys { get; set; }
        public bool remoteBanner { get; set; }
        public bool emailSignupRecaptcha { get; set; }
        public bool prebidSmart { get; set; }
        public bool inizio { get; set; }
    }

    public class Sharedadtargeting
    {
        public string ct { get; set; }
        public string url { get; set; }
        public string[] su { get; set; }
        public string edition { get; set; }
        public string[] tn { get; set; }
        public string p { get; set; }
        public string[] k { get; set; }
        public string sh { get; set; }
    }

    public class Lightboximages
    {
        public string id { get; set; }
        public string headline { get; set; }
        public bool shouldHideAdverts { get; set; }
        public string standfirst { get; set; }
        public object[] images { get; set; }
    }

    public class Abtests
    {
    }

    public class Reference
    {
        public string richlink { get; set; }
    }

    public class Pbindexsite
    {
        public string bp { get; set; }
        public int id { get; set; }
    }

    public class Libs
    {
        public string googletag { get; set; }
    }

    public class Switches1
    {
        public bool prebidAppnexusUkRow { get; set; }
        public bool mobileStickyPrebid { get; set; }
        public bool newsletterOnwards { get; set; }
        public bool abSignInGateMainVariant { get; set; }
        public bool commercialMetrics { get; set; }
        public bool prebidTrustx { get; set; }
        public bool scAdFreeBanner { get; set; }
        public bool abSignInGateCopyTestJan2023 { get; set; }
        public bool frontsBannerAds { get; set; }
        public bool prebidPermutiveAudience { get; set; }
        public bool compareVariantDecision { get; set; }
        public bool adFreeStrictExpiryEnforcement { get; set; }
        public bool enableSentryReporting { get; set; }
        public bool lazyLoadContainers { get; set; }
        public bool ampArticleSwitch { get; set; }
        public bool remarketing { get; set; }
        public bool keyEventsCarousel { get; set; }
        public bool verticalVideo { get; set; }
        public bool registerWithPhone { get; set; }
        public bool targeting { get; set; }
        public bool remoteHeader { get; set; }
        public bool deeplyReadSwitch { get; set; }
        public bool slotBodyEnd { get; set; }
        public bool prebidImproveDigitalSkins { get; set; }
        public bool ampPrebidOzone { get; set; }
        public bool extendedMostPopularFronts { get; set; }
        public bool emailInlineInFooter { get; set; }
        public bool showNewPrivacyWordingOnEmailSignupEmbeds { get; set; }
        public bool dcrNetworkFronts { get; set; }
        public bool iasAdTargeting { get; set; }
        public bool prebidAnalytics { get; set; }
        public bool extendedMostPopular { get; set; }
        public bool ampContentAbTesting { get; set; }
        public bool prebidCriteo { get; set; }
        public bool okta { get; set; }
        public bool abDeeplyReadArticleFooter { get; set; }
        public bool puzzlesBanner { get; set; }
        public bool imrWorldwide { get; set; }
        public bool acast { get; set; }
        public bool automaticFilters { get; set; }
        public bool twitterUwt { get; set; }
        public bool prebidAppnexusInvcode { get; set; }
        public bool ampPrebidPubmatic { get; set; }
        public bool a9HeaderBidding { get; set; }
        public bool prebidAppnexus { get; set; }
        public bool enableDiscussionSwitch { get; set; }
        public bool prebidXaxis { get; set; }
        public bool stickyVideos { get; set; }
        public bool interactiveFullHeaderSwitch { get; set; }
        public bool discussionAllPageSize { get; set; }
        public bool prebidUserSync { get; set; }
        public bool audioOnwardJourneySwitch { get; set; }
        public bool brazeTaylorReport { get; set; }
        public bool abConsentlessAds { get; set; }
        public bool externalVideoEmbeds { get; set; }
        public bool simpleReach { get; set; }
        public bool abIntegrateIma { get; set; }
        public bool callouts { get; set; }
        public bool carrotTrafficDriver { get; set; }
        public bool sentinelLogger { get; set; }
        public bool geoMostPopular { get; set; }
        public bool weAreHiring { get; set; }
        public bool relatedContent { get; set; }
        public bool thirdPartyEmbedTracking { get; set; }
        public bool prebidOzone { get; set; }
        public bool ampLiveblogSwitch { get; set; }
        public bool ampAmazon { get; set; }
        public bool borkFid { get; set; }
        public bool prebidAdYouLike { get; set; }
        public bool mostViewedFronts { get; set; }
        public bool optOutAdvertising { get; set; }
        public bool abSignInGateMainControl { get; set; }
        public bool headerTopNav { get; set; }
        public bool googleSearch { get; set; }
        public bool brazeSwitch { get; set; }
        public bool consentManagement { get; set; }
        public bool borkFcp { get; set; }
        public bool commercial { get; set; }
        public bool personaliseSignInGateAfterCheckout { get; set; }
        public bool redplanetForAus { get; set; }
        public bool prebidSonobi { get; set; }
        public bool idProfileNavigation { get; set; }
        public bool confiantAdVerification { get; set; }
        public bool discussionAllowAnonymousRecommendsSwitch { get; set; }
        public bool verticalVideoContainer { get; set; }
        public bool permutive { get; set; }
        public bool comscore { get; set; }
        public bool headerTopBarSearchCapi { get; set; }
        public bool ampPrebidCriteo { get; set; }
        public bool webFonts { get; set; }
        public bool europeNetworkFront { get; set; }
        public bool abBillboardsInMerch { get; set; }
        public bool prebidImproveDigital { get; set; }
        public bool offerHttp3 { get; set; }
        public bool ophan { get; set; }
        public bool crosswordSvgThumbnails { get; set; }
        public bool prebidTriplelift { get; set; }
        public bool weather { get; set; }
        public bool commercialOutbrainNewids { get; set; }
        public bool disableAmpTest { get; set; }
        public bool abLimitInlineMerch { get; set; }
        public bool serverShareCounts { get; set; }
        public bool abAdblockAsk { get; set; }
        public bool prebidPubmatic { get; set; }
        public bool autoRefresh { get; set; }
        public bool enhanceTweets { get; set; }
        public bool prebidIndexExchange { get; set; }
        public bool prebidOpenx { get; set; }
        public bool abElementsManager { get; set; }
        public bool prebidHeaderBidding { get; set; }
        public bool idCookieRefresh { get; set; }
        public bool serverSideLiveblogInlineAds { get; set; }
        public bool discussionPageSize { get; set; }
        public bool smartAppBanner { get; set; }
        public bool boostGaUserTimingFidelity { get; set; }
        public bool historyTags { get; set; }
        public bool mobileStickyLeaderboard { get; set; }
        public bool brazeContentCards { get; set; }
        public bool surveys { get; set; }
        public bool remoteBanner { get; set; }
        public bool emailSignupRecaptcha { get; set; }
        public bool prebidSmart { get; set; }
        public bool inizio { get; set; }
    }

    public class Tests
    {
    }

    public class Ophan
    {
        public string pageViewId { get; set; }
        public string browserId { get; set; }
    }

    public class Adblockers
    {
        public object[] onDetect { get; set; }
    }

    public class Modules
    {
        public Sentry sentry { get; set; }
    }

    public class Sentry
    {
    }

    public class Gadata
    {
        public string webTitle { get; set; }
        public string pillar { get; set; }
        public string section { get; set; }
        public string contentType { get; set; }
        public string commissioningDesks { get; set; }
        public string contentId { get; set; }
        public string authorIds { get; set; }
        public string keywordIds { get; set; }
        public string toneIds { get; set; }
        public string seriesId { get; set; }
        public string isHosted { get; set; }
        public string edition { get; set; }
        public string beaconUrl { get; set; }
    }

    public class Borkwebvitals
    {
    }
}
