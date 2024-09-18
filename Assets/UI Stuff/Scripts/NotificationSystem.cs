using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TweenAnimatorMultiple))]
public class NotificationSystem : MonoBehaviour
{
    private static Transform _cachedTransform, _playerTransform;
    private static ObjectAndComponentPool<Notification> _pool;
    private static Notification _notificationPrefab; 

    private void Awake() {
        _notificationPrefab = Resources.Load<Notification>("Notification");
        _pool = new ObjectAndComponentPool<Notification>(5, _notificationPrefab.gameObject, transform);
        _cachedTransform = transform;
    }

    private void Start() {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public static void SendNotification
    (NotificationType type, string content, Sprite icon = null, float offsetScaler = 1f, float animDurationScaler = 1f, float waitTime = 0f)
    {
        var notif = GetNotification();
        notif.transform.SetParent(_cachedTransform, true);
        notif.gameObject.SetActive(true);
        notif.Set(content, icon, _playerTransform, type, offsetScaler, animDurationScaler, waitTime);
    }

    static Notification GetNotification()
    {
        if(_pool == null)
        {
            _notificationPrefab = Resources.Load<Notification>("Notification");
            _pool = new ObjectAndComponentPool<Notification>(5, _notificationPrefab.gameObject, null);
        }
        return _pool.GetObjectWithComponent().Value;
    }

    private void OnDestroy() {
        _pool = new(0, _notificationPrefab.gameObject, null);
    }
}
public enum NotificationType
{
    Top, Left, Right, Bottom
}
