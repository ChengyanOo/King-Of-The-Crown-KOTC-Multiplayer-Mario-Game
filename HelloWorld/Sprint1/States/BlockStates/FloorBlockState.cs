using Sprint1.Entities;

namespace Sprint1.States.BlockStates
{

    public class FloorBlockState : BlockState
    {
        public FloorBlockState(BlockEntity entity, IBlockState previousState) : base(entity, previousState)
        { }

        // DO STAIR, FLOOR, BOTTOM PIPE BLOCKS (AND MAYBE USED?) HERE BECAUSE THEY ALL HAVE THE SAME BEHAVIOR
        // JUST DIFFERENT SPRITE ENUMS
    }
}
