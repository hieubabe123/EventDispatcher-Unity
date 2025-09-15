using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TT {
    public abstract class HUDBase<T> : SingletonMonoBehaviour<T> where T : MonoBehaviour {

        [Header("MAIN BACKGROUND")]
        [SerializeField] protected Image mainBackground;
        [SerializeField] private bool useBackgroundAimation;
        Color cacheColor;
        private Button mainBackgroundBtn;
        private float backgroundAlpha = -1;

        [SerializeField] List<PopupBase> frames;

        private Dictionary<Type, object> storage = new Dictionary<Type, object>();

        private void Awake() {
            UpdateStorageFromFrames();
        }

        private void Start() {
            if (mainBackground) {
                cacheColor = mainBackground.color;
                mainBackgroundBtn = mainBackground.GetComponent<Button>();
                mainBackgroundBtn?.onClick.AddListener(OnMainBackGroundClick);
            }
        }

        private void OnDestroy() {
            mainBackgroundBtn?.onClick.RemoveListener(OnMainBackGroundClick);

        }

        private void OnMainBackGroundClick() {

        }

        public void UpdateStorageFromFrames() {
            storage.Clear();

            foreach (PopupBase frame in frames) {
                if (frame != null) {

                    Type key = frame.GetType();
                    storage[key] = frame;
                }
            }
        }

        public T Get<T>() where T : PopupBase {
            if (storage.TryGetValue(typeof(T), out object value)) {
                return (T)value;
            }
            throw new Exception($"Can't find {typeof(T)}.");
        }



        public void Show<T>(bool useAnimation = true, Action callBack = null) where T : PopupBase {
            if (mainBackground) {
                mainBackground.gameObject.SetActive(true);
                if (useBackgroundAimation) {
                    Color newColor = mainBackground.color;
                    newColor.a = 0;

                    mainBackground.color = newColor;
                    mainBackground.DOFade(cacheColor.a, 0.3f);
                }
                else {

                }
            }
            Get<T>().Show(useAnimation, callBack);
        }

        public void Hide<T>(bool useAnimation = true, Action callBack = null) where T : PopupBase {
            if (mainBackground) {
                if (useBackgroundAimation) {
                    mainBackground.DOFade(0, 0.3f).OnComplete(() => {
                        mainBackground.gameObject.SetActive(false);
                    });
                }
                else {
                    mainBackground.gameObject.SetActive(false);
                }
            }

            Get<T>().Hide(useAnimation, callBack);
        }
    }

    public abstract class PopupBase : MonoBehaviour, IFrame {
        [Header("MAIN BACKGROUND")]
        [SerializeField] protected Image mainBackground;
        private Button mainBackgroundBtn;
        [Space]
        [Header("MAIN FRAME")]
        [SerializeField] protected Image mainFrame;
        [SerializeField] Button closeBtn;
        [Space]
        [Header("SHOW ANIMATION")]
        [SerializeField] protected float animationTime;
        [SerializeField] protected Ease showType;
        [SerializeField] protected Ease hideType;

        private void Start() {
            if (mainBackground) {
                mainBackground.gameObject.SetActive(true);
                mainBackgroundBtn = mainBackground.GetComponent<Button>();
                mainBackgroundBtn?.onClick.AddListener(OnMainBackGroundClick);
            }
            closeBtn?.onClick.AddListener(OnCloseClick);

            OnStart();
        }

        private void OnDestroy() {
            mainBackgroundBtn?.onClick.RemoveListener(OnMainBackGroundClick);
            closeBtn?.onClick.RemoveListener(OnCloseClick);
            OnDestroyFunc();
        }
        protected virtual void OnStart() { }
        protected virtual void OnDestroyFunc() { }

        private void OnMainBackGroundClick() {
            OnCloseClick();
        }

        protected virtual void OnCloseClick() {
            Hide(true);
        }

        public virtual void Show(bool useBGAnimation, bool useFrameAnim = true, Action callBack = null) {
            this.gameObject.SetActive(true);
            if (mainBackground) {
                mainBackground.gameObject.SetActive(true);
                if (useBGAnimation) {
                    Color cacheColor = mainBackground.color;
                    Color newColor = mainBackground.color;
                    newColor.a = 0;

                    mainBackground.color = newColor;
                    mainBackground.DOFade(cacheColor.a, 0.3f);
                }
            }


            if (mainFrame) {
                if (useFrameAnim) {
                    mainFrame.transform.localScale = Vector3.zero;
                    mainFrame.transform.DOScale(1f, animationTime).SetEase(showType)
                                       .OnComplete(() => callBack?.Invoke());
                }
                else {
                    callBack?.Invoke();
                }
            }
        }

        /// <summary>
        /// Call when you want show UI with doesn't need background Animation
        /// </summary>
        /// <param name="useFrameAnim"></param>
        /// <param name="callBack"></param>
        public virtual void Show(bool useFrameAnim = true, Action callBack = null) {
            Show(false, useFrameAnim, callBack);
        }


        public virtual void Hide(bool useFrameAnim = false, Action callBack = null) {
            if (mainFrame) {
                if (useFrameAnim) {
                    mainFrame.transform.DOScale(0f, animationTime).SetEase(hideType)
                                       .OnComplete(() => {
                                           callBack?.Invoke();
                                           this.gameObject.SetActive(false);
                                       });
                }
                else {
                    callBack?.Invoke();
                    this.gameObject.SetActive(false);
                }
            }
        }

    }

    public interface IFrame {
        void Show(bool useFrameAnim, Action callBack);
        void Hide(bool useFrameAnim, Action callBack);
    }
}