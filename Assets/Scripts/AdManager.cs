using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

//Use AdManager.Instance.ShowBanner(); in the scene you want banners

public class AdManager : MonoBehaviour {

	public static AdManager Instance { set; get;} //singleton
	// NEVER PUSH ACTUAL ID'S TO GITHUB
	public string bannerId;
	public string videoId;

	private void Start(){
		Instance = this;
		DontDestroyOnLoad (this.gameObject);

		Admob.Instance().initAdmob(bannerId, videoId);
		// Allow ads for testing mode - REMOVE WHEN PUBLISHING TODO
		Admob.Instance ().setTesting (true);
		Admob.Instance ().loadInterstitial ();
	}

	public void ShowBanner(){
		Admob.Instance ().showBannerRelative (AdSize.Banner, AdPosition.TOP_CENTER, 5, "defaultBanner");
	}

	public void ShowVideo(){
		if (Admob.Instance ().isInterstitialReady()) {
			Admob.Instance ().showInterstitial ();
		}
	}
}
