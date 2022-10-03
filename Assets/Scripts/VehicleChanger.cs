using UnityEngine;

public class VehicleChanger : MonoBehaviour
{
    [SerializeField] private GameObject vehicleObject;
    [SerializeField] private GameObject[] vehicles;

    private void Start()
    {
        int vehicleId = VehicleHelper.Vehicle;
        InstantiateVehicle(vehicleId);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) InstantiateVehicle(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) InstantiateVehicle(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) InstantiateVehicle(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) InstantiateVehicle(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) InstantiateVehicle(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) InstantiateVehicle(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) InstantiateVehicle(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) InstantiateVehicle(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) InstantiateVehicle(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) InstantiateVehicle(9);
        if (Input.GetKeyDown(KeyCode.Q)) InstantiateVehicle(10);
        if (Input.GetKeyDown(KeyCode.W)) InstantiateVehicle(11);
        if (Input.GetKeyDown(KeyCode.E)) InstantiateVehicle(12);
        if (Input.GetKeyDown(KeyCode.R)) InstantiateVehicle(13);
        if (Input.GetKeyDown(KeyCode.T)) InstantiateVehicle(14);
        if (Input.GetKeyDown(KeyCode.Y)) InstantiateVehicle(15);
        if (Input.GetKeyDown(KeyCode.U)) InstantiateVehicle(16);
        if (Input.GetKeyDown(KeyCode.I)) InstantiateVehicle(17);
        if (Input.GetKeyDown(KeyCode.O)) InstantiateVehicle(18);
        if (Input.GetKeyDown(KeyCode.P)) InstantiateVehicle(19);
        if (Input.GetKeyDown(KeyCode.A)) InstantiateVehicle(20);
        if (Input.GetKeyDown(KeyCode.S)) InstantiateVehicle(21);
    }

    private void InstantiateVehicle(int vehicleId)
    {
        if (vehicleObject) Destroy(vehicleObject);

        Vector3 position = new(0, 1, -20);
        vehicleObject = Instantiate(vehicles[vehicleId], position, Quaternion.identity);
        VehicleHelper.Vehicle = vehicleId;
    }
}
