﻿using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityCore {
    
    namespace Menu {

        public class PageController : MonoBehaviour
        {
            public static PageController instance;

            public bool debug;
            public PageType entryPage;
            public Page[] pages;

            private Hashtable m_Pages;
            private List<Page> m_OnList;
            private List<Page> m_OffList;

#region Unity Functions
            private void Awake() {
                if (!instance) {
                    instance = this;
                    m_Pages = new Hashtable();
                    m_OnList = new List<Page>();
                    m_OffList = new List<Page>();
                    RegisterAllPages();
                    TurnPageOn(entryPage);
                }
            }

            private void Update() {
                if (Input.GetKeyUp(KeyCode.F)) {
                    TurnPageOn(PageType.Loading);
                }
                if (Input.GetKeyUp(KeyCode.G)) {
                    TurnPageOff(PageType.Loading);
                }
            }
#endregion

#region Public Functions
            public async void TurnPageOn(PageType _type) {
                if (!PageExists(_type)) {
                    LogWarning("You are trying to turn a page on ["+_type+"] that has not been registered.");
                    return;
                }

                Page _page = GetPage(_type);
                _page.gameObject.SetActive(true);
                await _page.Animate(true);
            }

            public async void TurnPageOff(PageType _type) {
                if (!PageExists(_type)) {
                    LogWarning("You are trying to turn a page off ["+_type+"] that has not been registered.");
                    return;
                }

                Page _page = GetPage(_type);
                await _page.Animate(false);
                //_page.gameObject.SetActive(false);
            }
#endregion

#region Private Functions
            private void RegisterAllPages() {
                foreach(Page _page in pages) {
                    RegisterPage(_page);
                }
            }

            private void RegisterPage(Page _page) {
                if (PageExists(_page.type)) {
                    LogWarning("You are trying to register a page ["+_page.type+"] that has already been registered: <color=#f00>"+_page.gameObject.name+"</color>.");
                    return;
                }
                
                m_Pages.Add(_page.type, _page);
                Log("Registered new page ["+_page.type+"].");
            }

            private Page GetPage(PageType _type) {
                if (!PageExists(_type)) {
                    LogWarning("You are trying to get a page ["+_type+"] that has not been registered.");
                    return null;
                }

                return (Page)m_Pages[_type];
            }

            private bool PageExists(PageType _type) {
                return m_Pages.ContainsKey(_type);
            }

            private void Log(string _msg) {
                if (!debug) return;
                Debug.Log("[Page Controller]: "+_msg);
            }

            private void LogWarning(string _msg) {
                if (!debug) return;
                Debug.LogWarning("[Page Controller]: "+_msg);
            }
#endregion
        }
    }
}