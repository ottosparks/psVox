$PsVox::GenTime = 250;

function psVox::initGen(%this)
{
	if(isObject(%this.genQueue))
		return %this.genQueue;

	%this.genQueue = new ScriptObject(psVoxGenQueue){psVox = %this; jobs = 0;};
	return %this.genQueue;
}

function psVoxGenQueue::doFront(%this)
{
	if(%this.jobs == 0)
		return "";

	%func = %this.jobFunc0;
	for(%i = 0; %i < 16; %i++)
		%a[%i] = %this.jobArg0_[%i];

	if(isFunction(%func))
		%r = call(%func, %a0, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13, %a14, %a15);

	%this.jobFunc0 = "";
	for(%i = 15; %i >= 0; %i--)
		%this.jobArg0_[%i] = "";

	for(%i = 1; %i < %this.jobs; %i++)
	{
		%this.jobFunc[%i - 1] = %this.jobFunc[%i];
		for(%j = 0; %j < 16; %j++)
		{
			%this.jobArg[%i - 1, %j] = %this.jobArg[%i, %j];
			%this.jobArg[%i, %j] = "";
		}
		%this.jobFunc[%i] = "";
	}
	%this.jobs--;

	return %r;
}

function psVoxGenQueue::addJobToBack(%this, %func, %a0, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13, %a14, %a15)
{
	if(!isFunction(%func))
		return false;

	%this.jobFunc[%this.jobs] = %func;
	for(%i = 0; %i < 16; %i++)
		%this.jobArg[%this.jobs, %i] = %a[%i];

	%this.jobs++;
	return true;
}

function psVoxGenQueue::addJobToFront(%this, %func, %a0, %a1, %a2, %a3, %a4, %a5, %a6, %a7, %a8, %a9, %a10, %a11, %a12, %a13, %a14, %a15)
{
	if(!isFunction(%func))
		return false;

	for(%i = %this.jobs - 1; %i >= 0; %i--)
	{
		%this.jobFunc[%i + 1] = %this.jobFunc[%i];
		for(%j = 0; %j < 16; %j++)
			%this.jobArg[%i + 1, %j] = %this.jobArg[%i, %j];
	}

	%this.jobFunc0 = %func;
	for(%i = 0; %i < 16; %i++)
		%this.jobArg0_[%i] = %a[%i];

	%this.jobs++;
	return true;
}

function psVoxGenQueue::tick(%this)
{
	if(isEventPending(%this.tick))
		cancel(%this.tick);

	if(%this.jobs > 0)
		%this.doFront();

	%this.tick = %this.schedule($PsVox::GenTime, tick);
}



function psVoxGen_Chunk(%this, %x, %y, %z)
{
	%this.createChunk(%x, %y, %z SPC %z);
}

function psVoxGen_Chunks(%this, %startX, %startY, %startZ, %endX, %endY, %endZ, %cX, %cY, %cZ)
{
	%this.genQueue.addJobToBack(psVoxGen_Chunk, %this, %cX, %cY, %cZ);

	%cX++;
	if(%cX > %endX)
	{
		%cX = %startX;
		%cY++;
	}
	if(%cY > %endY)
	{
		%cY = %startY;
		%cZ++;
	}
	if(%cZ <= %endZ)
		%this.genQueue.addJobToFront(psVoxGen_Chunks, %this, %startX, %startY, %startZ, %endX, %endY, %endZ, %cX, %cY, %cZ);
}

function psVox::Gen_Chunks(%this, %startX, %startY, %startZ, %endX, %endY, %endZ)
{
	if(!isObject(%this.genQueue))
		%this.initGen();

	if(%startX > %endX)
	{
		%s = %startX;
		%e = %endX;
		%endX = %s;
		%startX = %e;
	}
	if(%startY > %endY)
	{
		%s = %startX;
		%e = %endX;
		%endX = %s;
		%startX = %e;
	}
	if(%startY > %endY)
	{
		%s = %startY;
		%e = %endY;
		%endY = %s;
		%startY = %e;
	}
	if(%startZ > %endZ)
	{
		%s = %startZ;
		%e = %endZ;
		%endZ = %s;
		%startZ = %e;
	}
	%this.genQueue.addJobToBack(psVoxGen_Chunks, %this, %startX, %startY, %startZ, %endX, %endY, %endZ, %cX, %cY, %cZ);
}