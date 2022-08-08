namespace Netstore.Infrastructure.CacheKeys;

public static class GroupCacheKeys
{
    public static string ListKey => "GroupList";

    public static string SelectListKey => "GroupSelectList";

    public static string GetKey(int groupId) => $"Group-{groupId}";

    public static string GetDetailsKey(int groupId) => $"GroupDetails-{groupId}";
}