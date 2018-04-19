

var planetMass = [
    { name: 'Сатурн', count:17},
    { name: 'Юпитер', count:16},
    { name: 'Уран', count:14},
    { name: 'Марс', count:2},
    { name: 'Нептун', count:2},
    { name: 'Земля', count:1},
    { name: 'Плутон', count:1}
]

function printList()
{
    document.write("<table border=1>")
    for (el of planetMass)
    {
        document.write("<tr>");
        document.write("<td>" + el.name.toString() + "</td>");
        document.write("<td>" + el.count.toString() + "</td>");
        document.write("</tr>");
    }
    document.write("</table>")
}
function getPlanets(idIn, idOut) {
    var input = parseInt(document.getElementById(idIn).value);
    var str = '';
    for (var planet of planetMass) {
        if (planet.count < input)
            str += (planet.name + ' ')
    }
    var output = document.getElementById(idOut);
    output.innerHTML = str;
}

//----------------------------------------------------------------------------------------------------------
function getArray(str)
{
    return str.split(' ');
}

function getMinMax(array)
{
    var min = array[0], max = array[0];
    var min_ind = 0, max_ind = 0;
    for (var i = 1; i < array.length; ++i)
    {
        if (parseInt(array[i]) < min) {
            min = array[i];
            min_ind = i;
        }
        if (parseInt(array[i]) > max) {
            max = array[i];
            max_ind = i;
        }
    }

    return [{ min: min, ind: min_ind }, { max: max, ind: max_ind }];
}

function getSum(array)
{
    var sumObj = {Chet:0, NChet:0 };
    for (var i = 0; i < array.length; ++i)
    {
        if (i % 2 === 0) sumObj.Chet += parseFloat(array[i]);
        else sumObj.NChet += parseFloat(array[i]);
    }

    return sumObj;
}

function zipMass(array)
{
    
    for (var i = array.length - 1; i >= 0; --i)
    {
        var el = array[i];
        if (Math.abs(parseFloat(el)) < 1)
            var a = array.splice(array.indexOf(el), 1);
        
    }


    return array;
}
function getStr(array)
{
    var str = "";
    for (var el of array)
    {
        if (el !== undefined)
        {
            str += (el.toString() + ' ');
        }

    }
    return str;
}
function mySortAsc(a, b) { return a - b; }
function mySortDesc(a, b) { return b - a;}

function getDigitArray(idIn, idOutMinMax, idOutSum, idOutSortAsc,idOutSortDesc, idOutZip)

{
    var strArray = document.getElementById(idIn).value;

    var minmaxArea = document.getElementById(idOutMinMax);
    var sumArea = document.getElementById(idOutSum);
    var sortAscArea = document.getElementById(idOutSortAsc);
    var sortDescArea = document.getElementById(idOutSortDesc);
    var zipArea = document.getElementById(idOutZip);

    var array = getArray(strArray);
    var minmax = getMinMax(array);
    var sum = getSum(array);

    minmaxArea.innerHTML = 'Mаксимальный элемент ' + minmax[1].max + ' под индексом ' + minmax[1].ind + '.\n Минимальный элемент ' + minmax[0].min + ' под индексом  ' + minmax[0].ind;
    sumArea.innerHTML = 'Сумма на чётных позициях: ' + sum.Chet +'\nСумма на нечётных позициях '+ sum.NChet;
    sortDescArea.innerHTML += array.sort(mySortDesc);
    sortAscArea.innerHTML += array.sort(mySortAsc);
    zipArea.innerHTML += getStr(zipMass(array));
    
}
//---------------------------------------------------------------------------------------------------
function foundMaxWord(str)
{
    var max = str[0], max_ind = 0;
    for (var i = 1; i < str.length; ++i)
    {
        if (str[i].length > max.length)
        {
            max = str[i];
            max_ind = i;
        }
    }

    return { max: max, ind: max_ind };
}

function foundUpperCaseWord(str)
{
    var array = [];
    for (word of str)
    {
        for (letter of word)
        {
            if (letter === letter.toUpperCase())
            {
                var v = array.push(word);
                break;
            }
        }
    }

    return array;
}

function getText(inId, outMaxId, outCountId, outUpperId)
{
    var input = document.getElementById(inId).value;


    var newinput =  input.replace(/,/g, '');
    var newinput1 = newinput.replace(/\./g, '');
    
    var array = getArray(newinput1);
    var count = array.length;

    var upperCaseCount = foundUpperCaseWord(array);
    var maxWord = foundMaxWord(array);

    var maxWordArea = document.getElementById(outMaxId);
    var countArea = document.getElementById(outCountId);
    var upperId = document.getElementById(outUpperId);

    maxWordArea.innerHTML += (maxWord.max + ' по индексу ' + maxWord.ind);
    countArea.innerHTML += count;
    upperId.innerHTML += upperCaseCount.join(', ');

}

