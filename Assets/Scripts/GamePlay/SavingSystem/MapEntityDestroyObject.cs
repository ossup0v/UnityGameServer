public class MapEntityDestroyObject : MapEntitySaveObject
{
    public int Id;

    private void OnDestroy()
    {
        RoomSendClient.DestroyMapItem(Id);
    }

    public override string Serialize()
    {
        return $"Id:{Id}" + base.Serialize();
    }
}
