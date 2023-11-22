using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class AssetLoader : MonoBehaviour
{
    public string baseUrl = "localhost:8080";
    public string bannersUrl = "/banners.json";
    public string mainMenu = "";
    public string loadingString = "Loading... ";
    public TMP_Text progress;
    public TMP_Text loading;
    public GameObject cardPrefab;

    public HttpClient httpClient = new();


    public List<Banner> banners = new();
    public List<Card> cards= new();


    public static AssetLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        StartCoroutine(GetBanners());
    }

    private IEnumerator GetBanners() {
        var request = UnityWebRequest.Get(baseUrl + bannersUrl);
        loading.text = "Banners...";
        float progressStatus = 0;
        yield return request.SendWebRequest();

        if (request.responseCode == 200) {
            string responseBody = request.downloadHandler.text;
            request.Dispose();
            progressStatus += 0.1f;
            progress.text = loadingString + Math.Floor(progressStatus * 100f) + "%";
            loading.text = "Banners Success";
            JavascriptPlugin.Log(responseBody);
            BannerData[] bannersData = JsonWrapper.FromJson<BannerData>(responseBody);

            foreach (var bannerData in bannersData) {
                Banner banner = new Banner() {
                    id = bannerData.id,
                    name = bannerData.name,
                };
                loading.text = "Loading Banner " + banner.name;
                yield return GetAssetBundle(banner, baseUrl + bannerData.assetBundleUrl);
                if (!string.IsNullOrEmpty(banner.error)) {
                    loading.text = "Banner Failed " + banner.name;
                    yield return null;
                } else {
                    progressStatus += (0.9f / bannersData.Length);
                    progress.text = loadingString + Math.Floor(progressStatus * 100f) + "%";
                    loading.text = "Loading Cards From " + banner.name;

                    yield return GetCards(banner, baseUrl + bannerData.cardsUrl);
                }
            }
        } else {
            loading.text = "Banners Success";
        }
    }

    private IEnumerator GetAssetBundle(Banner banner, string url)
    {
        var request = UnityWebRequestAssetBundle.GetAssetBundle(url, 0);
        yield return request.SendWebRequest();
        AssetBundle bundle = null;
        if (request.responseCode == 200)
        {
            bundle = DownloadHandlerAssetBundle.GetContent(request);
            if (bundle != null)
            {
                banner.bundle = bundle;
                JavascriptPlugin.Log($"SUCCESS BANNER {banner.name} BUNDLE {bundle.name}");
                JavascriptPlugin.Log(string.Join(",", bundle.GetAllAssetNames()));
                banners.Add(banner);
            } else {
                banner.error = "Error Loading Asset Bundle " + banner.name;
            }
        }

        if (bundle == null)
        {
            JavascriptPlugin.Log($"FAILED BUNDLE {banner.name} RESPONSE {request.responseCode}");
            banner.error = "Error Loading Banner " + banner.name;
        }
        request.Dispose();
    }

    private IEnumerator GetCards(Banner banner, string url) {
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.responseCode == 200) {
            string responseBody = request.downloadHandler.text;
            request.Dispose();
            JavascriptPlugin.Log(responseBody);
            CardData[] _cards = JsonWrapper.FromJson<CardData>(responseBody);
            foreach (var cardData in _cards) {
                Card.Rarity rarity = Card.Rarity.Common;
                switch(cardData.rarity) {
                    case "C":
                        rarity = Card.Rarity.Common;
                        break;
                    case "U":
                        rarity = Card.Rarity.Uncommon;
                        break;
                    case "R":
                        rarity = Card.Rarity.Rare;
                        break;
                    case "SR":
                        rarity = Card.Rarity.SR;
                        break;
                    case "UR":
                        rarity = Card.Rarity.UR;
                        break;
                }
                Card card = new Card() {
                    id= cardData.id,
                    name= cardData.name,
                    description= cardData.description,
                    rarity= rarity,
                };

                if (!string.IsNullOrEmpty(cardData.artworkAssetName))
                    card.artwork = banner.bundle.LoadAsset<Sprite>(cardData.artworkAssetName);

                if (!string.IsNullOrEmpty(cardData.backgroundAssetName))
                    card.background = banner.bundle.LoadAsset<Sprite>(cardData.backgroundAssetName);

                var cardInstance = Instantiate(cardPrefab);
                cardInstance.GetComponent<CardComponent>().Card = card;

                banner.cards.Add(card);
            }
        }
    }
}
