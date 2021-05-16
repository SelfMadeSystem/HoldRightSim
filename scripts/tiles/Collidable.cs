using Godot;

public interface Collidable {
    void EntityHit(Entity entity, KinematicCollision2D collision);
}