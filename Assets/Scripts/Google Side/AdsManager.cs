using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    const string Banner_ID         = "ca-app-pub-3940256099942544/6300978111";
    const string Video_ID          = "ca-app-pub-3940256099942544/1033173712";
    const string Rewarded_Video_ID = "ca-app-pub-3940256099942544/5224354917";

    private BannerView bannerView;
    private InterstitialAd video;
    private RewardedAd rewardedVideo;
    
    private bool IsRewardedAdClosed = false;

    private void Start()
    {
        var adsManagers = FindObjectsOfType<AdsManager>();
        if (adsManagers.Length > 1)
            Destroy(gameObject);
        else
        {
            DontDestroyOnLoad(gameObject);
            MobileAds.Initialize(initStatus => { });
            SceneManager.sceneLoaded += OnLevelLoad;
            RequestBannerAd();
            RequestVideoAd();
            RequestRewardedVideoAd();
            video.OnAdClosed += delegate { video.Destroy(); RequestVideoAd(); };
            rewardedVideo.OnAdClosed += delegate { RequestRewardedVideoAd(); IsRewardedAdClosed = true; };
        }
    }

    public bool IsAddClosed()
    {
        return IsRewardedAdClosed;
    }

    public void ShowSecondChanceAd()
    {
        IsRewardedAdClosed = false;
        rewardedVideo.Show();
    }

    public bool IsSecondChanceAdReady()
    {
        return rewardedVideo.IsLoaded();
    }

    public void ShowVideoAd()
    {
        if (video.IsLoaded())
            video.Show();
    }

    private void RequestVideoAd()
    {
        video = new InterstitialAd(Video_ID);
        AdRequest request = new AdRequest.Builder().Build();
        video.LoadAd(request);
    }

    private void RequestRewardedVideoAd()
    {
        rewardedVideo = new RewardedAd(Rewarded_Video_ID);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedVideo.LoadAd(request);
    }

    private void RequestBannerAd()
    {
        bannerView = new BannerView(Banner_ID, AdSize.SmartBanner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }

    private void OnLevelLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex != 1)
        {
            bannerView.Destroy();
            RequestBannerAd();
        }
        else
            bannerView.Destroy();
    }
}
