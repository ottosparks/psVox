if(!isObject(Brick4xCubeData))
	exec("Add-Ons/Brick_Large_Cubes/server.cs");
datablock fxDTSBrickData(Brick2xCubeData : Brick4xCubeData)
{
	brickFile = "./2xCube.blb";
	uiName = "2x Cube";
};

datablock fxDTSBrickData(Brick2xCubeM1Data : Brick2xCubeData)
{
	brickFile = "./2xCubem1.blb";
	uiName = "2x Cube -1f";
};

datablock itemData(voxelWrenchItem : wrenchItem)
{
	category = "Weapon";
	uiName = "Voxel Wrench";
	image = voxelWrenchImage;
	colorShiftColor = "0.3 0.3 0.3 1.";
};

datablock shapeBaseImageData(voxelWrenchImage : wrenchImage)
{	
	item = voxelWrenchItem;
	projectile = wrenchProjectile;
	colorShiftColor = "0.3 0.3 0.3 1";
	showBricks = 0;
};