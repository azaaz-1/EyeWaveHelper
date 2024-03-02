local directions = {
    "Up", "Left"
}
local TempleBigEyeWaveController = {}

TempleBigEyeWaveController =
{
    name = "EyeWaveHelper/TempleBigEyeWaveController",

    placements =
    {
        {
            name = "EyeWaveHelper/TempleBigEyeWaveController",
            data = {
                direction = "Left",
                maxInterval = 2,
                distance = 50,
                strength = 100,
                flag = ""
            },
        },
    },
};

TempleBigEyeWaveController.fieldInformation = {
        direction = {
            options = directions,
            editable = false
        }
}

TempleBigEyeWaveController.texture = "objects/eye"

return TempleBigEyeWaveController