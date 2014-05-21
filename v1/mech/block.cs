$PsVox::UpdateTick = 100;
$PsVox::UpdateReach = 1;

function psVoxBlockData::onBreak(%this, %obj, %player)
{
	//pass
}

// function psVoxBlockData::onUpdate(%this, %obj, %trigger)
// {
// 	//pass
// }

// function psVoxBlock::update(%this)
// {
// 	%this.type.schedule($PsVox::UpdateTick, onUpdate, %this, %this);
// 	%this.psVox.startBoxSearch(%this.pos, $PsVox::UpdateReach);
// 	while((%obj = %this.psVox.boxSearchNext()) != 0)
// 	{
// 		if(%obj == -1)
// 			continue;

// 		%obj.type.shedule(1$PsVox::UpdateTick, onUpdate, %obj, %this);
// 	}
// }

function psVox::startBoxSearch(%this, %pos, %range)
{
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%z = getWord(%pos, 2);
	%minX = %x - %range;
	%minY = %y - %range;
	%minZ = %z - %range;
	%maxX = %x + %range;
	%maxY = %y + %range;
	%maxZ = %z + %range;

	%min = %minX SPC %minY SPC %minZ;
	%max = %maxX SPC %maxY SPC %maxZ;
	%box = %min SPC %max;
	%this.boxSearch = %pos SPC %range SPC %box;
	%this.boxCurr = %min;
}

function psVox::startBoxSearch2(%this, %pos, %box)
{
	%x = getWord(%pos, 0);
	%y = getWord(%pos, 1);
	%z = getWord(%pos, 2);
	%minX = %x - getWord(%box, 0);
	%minY = %y - getWord(%box, 1);
	%minZ = %z - getWord(%box, 2);
	%maxX = %x + getWord(%box, 3);
	%maxY = %y + getWord(%box, 4);
	%maxZ = %z + getWord(%box, 5);

	%min = %minX SPC %minY SPC %minZ;
	%max = %maxX SPC %maxY SPC %maxZ;
	%box = %min SPC %max;
	%this.boxSearch = %pos SPC %range SPC %box;
	%this.boxCurr = %min;
}

function psVox::boxSearchNext(%this)
{
	if(%this.boxSearch $= "")
		return 0;

	%cX = getWord(%this.boxCurr, 0)+1;
	%cY = getWord(%this.boxCurr, 1);
	%cZ = getWord(%this.boxCurr, 2);

	%box = getWords(%this.boxSearch, 4, 9);
	%minX = getWord(%box, 0);
	%minY = getWord(%box, 1);
	%minZ = getWord(%box, 2);
	%maxX = getWord(%box, 3);
	%maxY = getWord(%box, 4);
	%maxZ = getWord(%box, 5);

	if(%cX > %maxX)
	{
		%cX = %minX;
		%cY++;
	}
	if(%cY > %maxY)
	{
		%cY = %minY;
		%cZ++;
	}
	if(%cZ > %maxZ)
	{
		%this.boxSearch = "";
		%this.boxCurr = "";
		return 0;
	}
	%this.boxCurr = %cX SPC %cY SPC %cZ;
	return %this.getBlock(%cX, %cY, %cZ);
}