using UnityEngine;

namespace Utility.Scripts
{
    public static class StumpRotationHelpers
    {
        //returns true if reached rotation direction on desired axis
        
        public static bool RotateTowardsOnY(this Transform transform, Vector3 destination, float speed, float deltaTime, out float anglesTurned)
        {
            var direction = destination - transform.position;
            direction.y = 0;
            var startYAngle = transform.eulerAngles.y;
            
            if (Vector3.Dot(transform.right, direction) > 0)
            {
                transform.Rotate(Vector3.up, speed * deltaTime, Space.World);
                anglesTurned = transform.eulerAngles.y - startYAngle;
                
                if (Vector3.Dot(transform.right, direction) > 0) return false;
                
                var angle1 = transform.eulerAngles;
                transform.forward = direction;
                transform.rotation = Quaternion.Euler(angle1.x, transform.eulerAngles.y, angle1.z);
                anglesTurned = transform.eulerAngles.y - startYAngle;
                return true;
            }
        
            transform.Rotate(Vector3.up, -speed * deltaTime, Space.World);
            anglesTurned = transform.eulerAngles.y - startYAngle;
            
            if (Vector3.Dot(transform.right, direction) < 0) return false;
        
            var angle2 = transform.eulerAngles;
            transform.forward = direction;
            transform.rotation = Quaternion.Euler(angle2.x, transform.eulerAngles.y, angle2.z);
            anglesTurned = transform.eulerAngles.y - startYAngle;
            return true;
        }
        
        public static bool RotateTowardsOnX(this Transform transform, Vector3 destination, float speed, float deltaTime, out float anglesTurned)
        {
            var direction = destination - transform.position;
            var direction0y = new Vector3(direction.x, 0, direction.z);
            var forward0y = new Vector3(transform.forward.x, 0, transform.forward.z);
            
            var startXAngle = transform.eulerAngles.x;

            if (Vector3.Dot(forward0y, direction0y) < 0)
            {
                direction0y = Vector3.Reflect(direction0y, forward0y);
                direction.x = direction0y.x;
                direction.z = direction0y.z;
            }
            
            if (Vector3.Dot(transform.up, direction) > 0)
            {
                transform.Rotate(Vector3.right, -speed * deltaTime, Space.Self);
                anglesTurned = transform.eulerAngles.x - startXAngle;

                if (Vector3.Dot(transform.up, direction) > 0) return false;
                
                var angle1 = transform.eulerAngles;
                transform.forward = direction;
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle1.y, angle1.z);
                anglesTurned = transform.eulerAngles.x - startXAngle;
                return true;
            }
            
            transform.Rotate(Vector3.right, speed * deltaTime, Space.Self);
            anglesTurned = transform.eulerAngles.x - startXAngle;

            if (Vector3.Dot(transform.up, direction) < 0) return false;
            
            var angle2 = transform.eulerAngles;
            transform.forward = direction;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle2.y, angle2.z);
            anglesTurned = transform.eulerAngles.x - startXAngle;
            return true;
        }
    }
}